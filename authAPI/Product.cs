using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace authAPI
{
	public class Product
	{
		public int Id { get; set; }

		public string ProductName { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

        [JsonIgnore]
        public Auth Auth { get; set; }

		public int AuthId { get; set; }

		[JsonIgnore]
		public List<Order> Orders { get; set; }
	}

}

