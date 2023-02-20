using System;
using System.Text.Json.Serialization;

namespace authAPI
{
	public class Voucher
	{
		public int Id { get; set; }

		public string VoucherCode { get; set; }

		public double Price { get; set; }

		public int Used { get; set; } = 1;

		public string UsedBy { get; set; } = string.Empty;

		public DateTime ExpireDate { get; set; }

		[JsonIgnore]
		public Auth Auth { get; set; }

		public int AuthId { get; set; }

		public int OrderId { get; set; }


	}
}

