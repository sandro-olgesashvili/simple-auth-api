using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace authAPI
{
	public class Product
	{
		public int Id { get; set; }

		public string ProductName { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

		public List<Order> Orders { get; set; }
	}

}

