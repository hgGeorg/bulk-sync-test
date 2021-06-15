using System;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulkSyncTest
{
    #region Multiple periods/dots
    public class EntityWithMultipleDotsInTableName
    {
        public int Id { get; set; }
    }
    #endregion

    #region Inheritance
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
    #endregion

    #region Complex
    public class Complex
    {
        public int Id { get; set; }
        public OwnedA OwnedA { get; set; }
        public OwnedB OwnedB { get; set; }
    }

    public class OwnedA
    {
        public int? ReferencedId { get; set; }
        public Referenced Referenced { get; set; }
    }

    public class Referenced
    {
        public int Id { get; set; }
    }

    public class OwnedB
    {
        public string Content { get; set; }
    }
    #endregion

    public class MyDbContext : DbContext
    {
        public DbSet<EntityWithMultipleDotsInTableName> DottedTableName { get; set; }
        public DbSet<Base> Inheritance { get; set; }
        public DbSet<Complex> ProfileTemplates { get; set; }

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

            builder.Entity<Complex>(entity =>
            {
                entity.OwnsOneRequired(e => e.OwnedA, pm =>
                {
                    pm.HasOne(m => m.Referenced).WithMany().HasForeignKey(m => m.ReferencedId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
                });
                entity.OwnsOneRequired(e => e.OwnedB);
            });

            builder.Entity<Referenced>();

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

            //TableNameWithMultipleDots.Demo(context, syncContext);
            //Inheritance.Demo(context, syncContext);
            ComplexModel.Demo(context, syncContext);
        }
    }

    public static class Helpers
    {
        public static OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOneRequired<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> entity, Expression<Func<TEntity, TRelatedEntity>> navigationExpression)
            where TEntity : class
            where TRelatedEntity : class
        {
            var builder = entity.OwnsOne(navigationExpression);
            entity.Navigation(navigationExpression).IsRequired();
            return builder;
        }

        public static EntityTypeBuilder<TEntity> OwnsOneRequired<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> entity, Expression<Func<TEntity, TRelatedEntity>> navigationExpression, Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction)
            where TEntity : class
            where TRelatedEntity : class
        {
            var builder = entity.OwnsOne(navigationExpression, buildAction);
            entity.Navigation(navigationExpression).IsRequired();
            return builder;
        }
    }
}
