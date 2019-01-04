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

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<ShoppingCartProduct> ShoppingCartProducts { get; set; }
        
        public DbSet<XeonUserFavoriteProduct> XeonUserFavoriteProducts { get; set; }

        public DbSet<PartnerRequest> PartnerRequests { get; set; }

        public DbSet<UserRequest> UserRequests { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public XeonDbContext(DbContextOptions<XeonDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderProduct>().HasKey(x => new { x.OrderId, x.ProductId });

            builder.Entity<ShoppingCartProduct>().HasKey(x => new { x.ProductId, x.ShoppingCartId });

            builder.Entity<CategoryProduct>().HasKey(x => new { x.ChildCategoryId, x.ProductId });

            builder.Entity<XeonUserFavoriteProduct>().HasKey(x => new { x.ProductId, x.XeonUserId });

            builder.Entity<Product>()
                   .HasOne(x => x.ChildCategory)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.ChildCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ShoppingCart>()
                   .HasOne(x => x.User)
                   .WithOne(x => x.ShoppingCart)
                   .HasForeignKey<XeonUser>(x => x.ShoppingCartId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Company>()
                  .HasOne(x => x.XeonUser)
                  .WithOne(x => x.Company)
                  .HasForeignKey<XeonUser>(x => x.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<XeonUser>()
                  .HasOne(x => x.PartnerRequest)
                  .WithOne(x => x.XeonUser)
                  .HasForeignKey<PartnerRequest>(x => x.XeonUserId);

            base.OnModelCreating(builder);
        }
    }
}