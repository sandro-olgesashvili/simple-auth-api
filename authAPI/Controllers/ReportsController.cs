using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using authAPI.Service;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly DataContext _context;

        private readonly IUserService _userService;

        public ReportsController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
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
    }
}

