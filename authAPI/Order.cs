using System;
using System.Text.Json.Serialization;
namespace authAPI
{
	public class Order
	{
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string ImageSrc { get; set; }

        public Product Product { get; set; }

        public int ProductId { get; set; }

        public Auth Auth { get; set; }

        public int AuthId { get; set; }


    }
}

