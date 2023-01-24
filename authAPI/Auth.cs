using System;
using System.Text.Json.Serialization;

namespace authAPI
{
	public class Auth
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		[JsonIgnore]
		public List<Product> Products { get; set; }
	}
}

