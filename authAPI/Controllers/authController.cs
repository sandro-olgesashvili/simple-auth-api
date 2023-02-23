using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using authAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    public class authController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public authController(DataContext context, IConfiguration configuration, IUserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<TokenRoleDto>> Post([FromBody] AuthDto req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username).FirstOrDefault();

            if (user == null || user.Password != req.Password)
            {
                return Ok(false);
            }

            TokenRoleDto token = new TokenRoleDto()
            {
                Token = CreateToken(user),
                Role = user.Role,
                Id = user.Id,
            };

            return Ok(token);

        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register([FromBody] Auth req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username).FirstOrDefault();
            if (user != null)
            {
                return Ok(false);
            } else
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

            if (user == null || user.Username != req.Username)
            {
                return Ok(false);
            } else
            {
                user.Password = req.Password;
                await _context.SaveChangesAsync();

                return Ok(true);
            }

        }



        [HttpPost("product"), Authorize(Roles = "admin, seller")]
        public async Task<ActionResult<List<Product>>> AddProduct([FromBody] ProductDto req)
        {
            var checkProduct = _context.Products.Where(x => x.ProductName == req.ProductName).FirstOrDefault();

            var username = _userService.GetMyName();

            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (user == null)
            {
                return Ok(false);
            }

            if (checkProduct != null)
            {
                return Ok(false);
            }


            if (req.ProductName.Trim().Length < 1 || req.Quantity < 1 || req.Price < 1)
            {
                return Ok(false);
            }

            var newProduct = new Product()
            {
                ProductName = req.ProductName,
                Quantity = req.Quantity,
                Price = req.Price,
                AuthId = user.Id,
            };

            _context.Products.Add(newProduct);

            await _context.SaveChangesAsync();

            var product = _context.Products.Where(x => x.ProductName == req.ProductName).FirstOrDefault();


            return Ok(product);
        }



        [HttpGet("products"), Authorize]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }


        [HttpDelete("products"), Authorize(Roles = "admin, seller")]
        public async Task<ActionResult<bool>> DeleteProduct([FromQuery] ProductDto req)
        {

            var username = _userService.GetMyName();

            if (username == "admin")
            {
                var product = _context.Products.Where(x => x.ProductName == req.ProductName).FirstOrDefault();

                if (product == null)
                {
                    return Ok(false);
                }

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();


                return Ok(true);
            } else
            {
                var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

                var product = _context.Products.Where(x => x.ProductName == req.ProductName && x.AuthId == user.Id).FirstOrDefault();

                if (user == null) return Ok(false);

                if (product == null) return Ok(false);

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                return Ok(true);
            }


        }


        [HttpGet("orders"), Authorize]
        public async Task<ActionResult<List<Order>>> GetOrder()
        {
            var username = _userService.GetMyName();

            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (user == null)
            {
                return Ok(false);
            }

            var orderPorduct = await _context.Orders.Where(x => x.AuthId == user.Id).ToListAsync();

            return Ok(orderPorduct);
        }


        [HttpPost("orders"), Authorize]
        public async Task<ActionResult<Order>> PostOrder([FromBody] OrderDto req)
        {
            var username = _userService.GetMyName();

            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (user == null)
            {
                return Ok(false);
            }

            var product = _context.Products.Where(x => x.ProductName == req.ProductName).FirstOrDefault();

            if (product == null)
            {
                return Ok(false);
            }

            var newOrder = new Order()
            {
                ProductName = product.ProductName,

                Quantity = 1,

                Price = product.Price,

                AuthId = user.Id,

                ProductId = product.Id

            };

            product.Quantity = product.Quantity - 1;

            _context.Orders.Add(newOrder);

            await _context.SaveChangesAsync();

            var sendOrder = _context.Orders.Where(x => x.ProductName == req.ProductName).FirstOrDefault();

            return Ok(sendOrder);
        }


        [HttpDelete("orders"), Authorize]
        public async Task<ActionResult<bool>> DeleteOrder([FromQuery] OrderDto req)
        {
            var username = _userService.GetMyName();

            var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            if (user == null)
            {
                return Ok(false);
            }

            var checkOrder = _context.Orders.Where(x => x.ProductName == req.ProductName && x.AuthId == user.Id).FirstOrDefault();
            var product = _context.Products.Where(x => x.ProductName == req.ProductName).FirstOrDefault();
            var vouchers = _context.Vouchers.Where(x => x.OrderId == checkOrder.Id).ToList();



            if(vouchers != null)
            {
                foreach (var item in vouchers)
                {
                    item.Used = 1;
                    item.UsedBy = string.Empty;
                    item.OrderId = 0;
                }

            }


            product.Quantity = product.Quantity + checkOrder.Quantity;

            if (checkOrder == null)
            {
                return Ok(false);
            }

            _context.Orders.Remove(checkOrder);

            await _context.SaveChangesAsync();

            return Ok(true);

        }

        [HttpPatch("save"), Authorize]
        public async Task<ActionResult<bool>> SaveChange([FromBody] List<Order> req)
        {

            foreach (var item in req)
            {
                var user = _context.Users.Where(x => x.Id == item.AuthId).FirstOrDefault();
                var product = _context.Products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                var order = _context.Orders.Where(x => x.Id == item.Id).FirstOrDefault();

                var num = product.Quantity + order.Quantity;

                product.Quantity = num - item.Quantity;

                order.Quantity = item.Quantity;


                await _context.SaveChangesAsync();
            }


            return Ok(true);
        }


        [HttpPatch("update"), Authorize(Roles = "admin, seller")]
        public async Task<ActionResult<bool>> ProductUpdate([FromBody] Product req)
        {
            var username = _userService.GetMyName();

            if (req.Quantity <= 0 || req.Price <= 0) return Ok(false);

            if (username == "admin")
            {
                var product = _context.Products.Where(x => x.Id == req.Id).FirstOrDefault();

                var products = await _context.Products.Where(x => x.Id != req.Id).ToListAsync();

                foreach (var item in products)
                {
                    if (item.ProductName == req.ProductName) return Ok(false);
                }

                if (product == null) return Ok(false);

                product.ProductName = req.ProductName;

                product.Quantity = req.Quantity;

                product.Price = req.Price;

                await _context.SaveChangesAsync();

                return Ok(true);

            } else
            {
                var user = _context.Users.Where(x => x.Username == username).FirstOrDefault();

                if (user == null) return Ok(false);

                var product = _context.Products.Where(x => x.Id == req.Id && x.AuthId == user.Id).FirstOrDefault();

                var products = await _context.Products.Where(x => x.Id != req.Id).ToListAsync();

                foreach (var item in products)
                {
                    if (item.ProductName == req.ProductName) return Ok(false);
                }


                if (product == null) return Ok(false);

                product.ProductName = req.ProductName;

                product.Quantity = req.Quantity;

                product.Price = req.Price;

                await _context.SaveChangesAsync();

                return Ok(product);

            }
        }

        [HttpGet("users"), Authorize(Roles = "admin")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var username = _userService.GetMyName();


            var users = await _context.Users.Select(x => new UserDto() {Name = x.Username, Code = x.Id }).ToListAsync();
            

            return Ok(users);
        }


        private string CreateToken(Auth user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}

