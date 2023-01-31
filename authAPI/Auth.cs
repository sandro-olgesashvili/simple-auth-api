using System;
using System.Text.Json.Serialization;

namespace authAPI
{
	public class Auth
	{
		public int Id { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string Role { get; set; } = "user";

		[JsonIgnore]
		public List<Order> Orders { get; set; }
	}
}


