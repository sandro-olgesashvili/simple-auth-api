using System;
namespace authAPI.Service
{
	public class VoucherService: IVoucherService
    {
        
        public string Get()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 16;

            var random = new Random();

            return new string(Enumerable.Repeat(characters, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

