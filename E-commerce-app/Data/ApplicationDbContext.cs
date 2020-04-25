using System;
using System.Collections.Generic;
using System.Text;
using E_commerce_app.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_app.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartProduct> ShoppingCartProducts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(a => a.Categories)
                .WithOne(a => a.Product)
                .HasForeignKey(a => a.ProductId);
            modelBuilder.Entity<Product>()
                .HasMany(a => a.ShoppingCartProducts)
                .WithOne(a => a.Product)
                .HasForeignKey(a => a.ProductId);
            modelBuilder.Entity<Product>()
                .HasMany(a => a.TransactionProducts)
                .WithOne(a => a.Product)
                .HasForeignKey(a => a.ProductId);
            modelBuilder.Entity<Category>()
                .HasMany(a => a.Products)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);
            modelBuilder.Entity<Review>()
                .HasOne(a => a.Product)
                .WithMany(a => a.Reviews)
                .HasForeignKey(a => a.ProductId);
            modelBuilder.Entity<Review>()
                .HasOne(a => a.User)
                .WithMany(a => a.Reviews)
                .HasForeignKey(a => a.UserId);
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(a => a.User)
                .WithMany(a => a.ShoppingCarts)
                .HasForeignKey(a => a.UserId);
            modelBuilder.Entity<ShoppingCart>()
                .HasMany(a => a.ShoppingCartProducts)
                .WithOne(a => a.ShoppingCart)
                .HasForeignKey(a => a.ShoppingCartId);
            modelBuilder.Entity<Transaction>()
                .HasOne(a => a.User)
                .WithMany(a => a.Transactions)
                .HasForeignKey(a => a.UserId);
            modelBuilder.Entity<Transaction>()
                .HasMany(a => a.TransactionProducts)
                .WithOne(a => a.Transaction)
                .HasForeignKey(a => a.TransactionId);
        }
        }
}
