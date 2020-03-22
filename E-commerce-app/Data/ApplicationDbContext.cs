using System;
using System.Collections.Generic;
using System.Text;
using E_commerce_app.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_app.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
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
            modelBuilder.Entity<Category>()
                .HasMany(a => a.Products)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);
        }
        }
}
