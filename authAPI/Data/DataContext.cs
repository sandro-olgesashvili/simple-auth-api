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
	}
}


	