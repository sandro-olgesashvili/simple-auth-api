using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using authAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly DataContext _context;

        private readonly IUserService _userService;

        private readonly IWebHostEnvironment _env;


        public ReportsController(DataContext context, IUserService userService, IWebHostEnvironment env)
        {
            _context = context;
            _userService = userService;
            _env = env;
        }

        [HttpGet, Authorize(Roles ="admin")]
        public async Task<ActionResult<bool>> Get()
        {

            var soldProducts = await _context.SoldProducts.ToListAsync();

            var soldProductsByProduct = soldProducts.GroupBy(sp => sp.ProductName);

            var productSales = soldProductsByProduct.Select(g => new {
                ProductName = g.Key,
                TotalSales = g.Sum(sp => sp.Quantity * sp.Price),
                Quantity = g.Sum(sp => sp.Quantity)
            }).OrderByDescending(x => x.TotalSales).ToList();


            return Ok(productSales);
        }

        [HttpGet("date"), Authorize(Roles ="admin")]
        public async Task<ActionResult<bool>> GetByDate([FromQuery] DateTime start, [FromQuery] DateTime end)
        {

            var soldProducts = await _context.SoldProducts.Where(x => x.DateTime <= end && start <= x.DateTime).ToListAsync();

            var soldProductsByProduct = soldProducts.GroupBy(sp => sp.ProductName);

            var productSales = soldProductsByProduct.Select(g => new {
                ProductName = g.Key,
                TotalSales = g.Sum(sp => sp.Quantity * sp.Price),
                Quantity = g.Sum(sp => sp.Quantity)
            }).OrderByDescending(x => x.TotalSales).ToList();


            return Ok(productSales);
        }
    }
}

