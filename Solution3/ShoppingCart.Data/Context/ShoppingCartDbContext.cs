using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Data.Context
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
