using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XeonComputers.Models;

namespace XeonComputers.Data
{
    public class XeonDbContext : IdentityDbContext<XeonUser>
    {
        public DbSet<City> Cities { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ParentCategory> ParentCategories { get; set; }

        public DbSet<ChildCategory> ChildCategories { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public XeonDbContext(DbContextOptions<XeonDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderProduct>().HasKey(x => new { x.OrderId, x.ProductId });

            builder.Entity<CategoryProduct>().HasKey(x => new { x.ChildCategoryId, x.ProductId });

            builder.Entity<Product>()
                   .HasOne(x => x.ChildCategory)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.ChildCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}