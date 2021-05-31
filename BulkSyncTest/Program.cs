using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BulkSyncTest
{
    public class EntityWithMultipleDotsInTableName
    {
        public int Id { get; set; }
    }

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
        public DbSet<EntityWithMultipleDotsInTableName> DottedTableName { get; set; }
        public DbSet<Base> Inheritance { get; set; }

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
            builder.Entity<EntityWithMultipleDotsInTableName>().ToTable("Entity.With.Multiple.Dots.In.Table.Name");

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

            TableNameWithMultipleDots.Demo(context, syncContext);
            Inheritance.Demo(context, syncContext);
        }
    }
}
