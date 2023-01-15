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
        public async Task<ActionResult<bool>> Post([FromBody] Auth req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username).FirstOrDefault();

            if (user == null || user.Password != req.Password)
            {
                return Ok(false);
            } else
            {
                return Ok(true);
            }
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
        public async Task<ActionResult<bool>> Update([FromBody] Update req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username).FirstOrDefault();

            if(user == null || (user.Username == req.Username && user.Password != req.Password))
            {
                return Ok(false);
            } else
            {
                user.Password = req.NewPassword;
                await _context.SaveChangesAsync();

                return Ok(true);
            }
                    
        }
    }
}

