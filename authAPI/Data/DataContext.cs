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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.Auth)
                .HasForeignKey(o => o.AuthId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.Product)
                .HasForeignKey(o => o.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Auth)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.AuthId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId);

            modelBuilder.Entity<Voucher>()
                .HasOne(o => o.Auth)
                .WithMany(u => u.Vouchers)
                .HasForeignKey(o => o.AuthId);
        }
    }
}


	