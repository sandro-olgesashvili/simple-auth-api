using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using authAPI.Service;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    public class VoucherController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IVoucherService _voucherService;


        public VoucherController(DataContext context, IUserService userService, IVoucherService voucherService)
        {
            _context = context;
            _userService = userService;
            _voucherService = voucherService;
        }


        [HttpGet, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Voucher>>> Get()
        {
            var vouchers = await _context.Vouchers.ToListAsync();

            return Ok(vouchers);
        }

        [HttpPost, Authorize(Roles ="admin")]
        public async Task<ActionResult<Voucher>> PostVoucher([FromBody] VoucherDto req)
        {
            var username = _userService.GetMyName();

            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (user == null) return Ok(false);

            if (req.Price < 1) return Ok(false);

            var voucher = _voucherService.Get();

            var voucherCreate = new Voucher()
            {
                VoucherCode = voucher,

                Price = req.Price,

                Auth = user,

                AuthId = user.Id
            };

            _context.Vouchers.Add(voucherCreate);

            await _context.SaveChangesAsync();

            var newVoucher = _context.Vouchers.Where(x => x.VoucherCode == voucher).FirstOrDefault();

            return Ok(newVoucher);
        }

        [HttpPost("usevoucher"), Authorize]
        public async Task<ActionResult<Order>> UseVoucher([FromBody] VoucherUseDto req)
        {
            var username = _userService.GetMyName();

            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (user == null) return Ok(false);

            var order = _context.Orders.Where(x => x.Id == req.OrderId).FirstOrDefault();

            if (order == null) return Ok(false);

            var voucher = _context.Vouchers.Where(x => x.VoucherCode == req.VoucherCode).FirstOrDefault();

            if (voucher == null || voucher.Used == true) return Ok(false);

            voucher.Used = true;

            order.Price = order.Price - voucher.Price;

            await _context.SaveChangesAsync();

            var newOrder = _context.Orders.Where(x => x.Id == req.OrderId).FirstOrDefault();

            return Ok(newOrder);
        }


        [HttpDelete, Authorize(Roles ="admin")]
        public async Task<ActionResult<Voucher>> DeleteVoucher([FromQuery] string voucherCode)
        {
            var username = _userService.GetMyName();

            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (user == null) return Ok(false);

            var voucher = _context.Vouchers.Where(x => x.VoucherCode == voucherCode).FirstOrDefault();

            if (voucher == null) return Ok(false);

            _context.Vouchers.Remove(voucher);

            await _context.SaveChangesAsync();

            return Ok(voucher);
        }

    }
}

