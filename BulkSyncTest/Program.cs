using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BulkSyncTest
{
    public abstract class Base
    {
        public int Id { get; set; }
        public string SomeBaseProperty { get; set; }
    }

    public class InheritEmpty : Base
    {
    }

    public class InheritA : Base
    {
        public string SomeOtherPropA { get; set; }
    }

    public class InheritB : Base
    {
        public string SomeOtherPropB { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public DbSet<Base> Table { get; set; }

        public MyDbContext() : base(new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlServer("")
                .Options)
        {
        }
        
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Base>();
            builder.Entity<InheritEmpty>().HasBaseType<Base>();
            builder.Entity<InheritA>().HasBaseType<Base>();
            builder.Entity<InheritB>().HasBaseType<Base>();
            base.OnModelCreating(builder);
        }
    }

    public class MyDBContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=sync_test");

            return new MyDbContext(optionsBuilder.Options);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=sync_test;Trusted_Connection=False;TrustServerCertificate=True;Integrated Security=SSPI")
                .Options;
            var syncOptions = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=other_db;Trusted_Connection=False;TrustServerCertificate=True;Integrated Security=SSPI")
                .Options;
            var context = new MyDbContext(options);
            context.Database.EnsureCreated();
            var syncContext = new MyDbContext(syncOptions);
            syncContext.Database.EnsureCreated();


            if (!context.Table.Any())
            {
                // add items to table if empty
                for (int i = 0; i < 100; i++)
                {
                    var _ = (i % 3) switch
                    {
                        2 => context.Table.Add(new InheritEmpty()),
                        1 => context.Table.Add(new InheritA()),
                        0 => context.Table.Add(new InheritB()),
                        _ => throw new NotImplementedException()
                    };
                }
                context.SaveChanges();
            }

            try
            {
                syncContext.BulkSynchronize(context.Table, options =>
                {
                    options.SynchronizeKeepidentity = true;
                });
            }
            catch (InvalidCastException exc)
            {
                // relevant for this bug report
                Console.WriteLine(exc);
            }
        }
    }
}
