using System;
using System.Text.Json.Serialization;

namespace authAPI
{
	public class Voucher
	{
		public int Id { get; set; }

		public string VoucherCode { get; set; }

		public double Price { get; set; }

		public bool Used { get; set; } = false;

		[JsonIgnore]
		public Auth Auth { get; set; }

		public int AuthId { get; set; }
	}
}

