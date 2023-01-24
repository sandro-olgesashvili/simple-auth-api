using System;
namespace authAPI
{
	public class Product
	{
		public int Id { get; set; }

		public string ProductName { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

		public Auth Auth { get; set; }

		public int AuthId { get; set; }
	}

}

