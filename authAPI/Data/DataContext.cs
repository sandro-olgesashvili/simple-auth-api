using System;
using Microsoft.EntityFrameworkCore;

namespace authAPI.Data
{
	public class DataContext : DbContext
    {
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<Auth> Users { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Order> Orders { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<SoldProduct> SoldProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth>()
                .HasMany(a => a.Orders)
                .WithOne(o => o.Auth)
                .HasForeignKey(o => o.AuthId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auth>()
                .HasMany(a => a.SoldProducts)
                .WithOne(o => o.Auth)
                .HasForeignKey(o => o.AuthId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.Product)
                .HasForeignKey(o => o.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Auth)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AuthId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.Auth)
                .WithMany(a => a.Vouchers)
                .HasForeignKey(v => v.AuthId);

            modelBuilder.Entity<SoldProduct>()
                .HasOne(a => a.Auth)
                .WithMany(s => s.SoldProducts)
                .HasForeignKey(s => s.AuthId);

        }
    }
}


	