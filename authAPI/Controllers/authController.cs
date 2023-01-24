using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    public class authController : Controller
    {
        private readonly DataContext _context;

        public authController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<AuthDto>> Post([FromBody] Auth req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username).FirstOrDefault();

            if (user == null || user.Password != req.Password)
            {
                return Ok(false);
            }

            AuthDto authDto = new AuthDto
            {
                Id = user.Id,
                Username = user.Username
            };

            return Ok(authDto);
            
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register([FromBody] Auth req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username).FirstOrDefault();
            if(user != null)
            {
                return Ok(false);
            }else
            {
                _context.Users.Add(req);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
        }
        [HttpPut]
        public async Task<ActionResult<bool>> Update([FromBody] Auth req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username).FirstOrDefault();

            if(user == null || user.Username != req.Username)
            {
                return Ok(false);
            } else
            {
                user.Password = req.Password;
                await _context.SaveChangesAsync();

                return Ok(true);
            }
                    
        }
        [HttpPost("product")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            var user = await _context.Users.FindAsync(product.AuthId);

            var newProd = new Product()
            {
                ProductName = product.ProductName,
                Quantity = product.Quantity,
                Price = product.Price,
                Auth = user,
                AuthId = product.AuthId,
            };

            _context.Products.Add(newProd);

            await _context.SaveChangesAsync();

            return Ok(newProd);

        }

        [HttpGet("products")]
        public async Task<ActionResult<List<Product>>> GetProduct([FromQuery] AuthDto req)
        {
            var product = await _context.Products.Where(x => x.AuthId == req.Id).ToListAsync();

            return Ok(product);
        }
        [HttpDelete("products")]
        public async Task<ActionResult<bool>> DeleteProduct([FromQuery] UserProductDto req)
        {
            var user = await _context.Users.FindAsync(req.UserId);


            if(user == null)
            {
                return Ok(false);
            }

            var product = _context.Products.Where(x => x.Id == req.ProductId).FirstOrDefault();

            if(product == null)
            {
                return Ok(false);
            }


            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return Ok(true);
        }
        
    }
}

