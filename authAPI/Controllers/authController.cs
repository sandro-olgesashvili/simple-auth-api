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
        private static List<Auth> auths = new List<Auth>
        {
            new Auth{Username = "giorgi", Password= "123456"},
            new Auth{Username = "daviti", Password= "123456"},
        };


        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody]Auth authRequest)
        {
            var auth = auths.Find((i) => i.Username == authRequest.Username);

            if(auth == null || auth.Password != authRequest.Password)
            {
                return Ok(false);
            }
            else
            {
                return Ok(true);
            }

        }
        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register([FromBody] Auth authRequest)
        {
            var acc = auths.Find(i => i.Username == authRequest.Username);

            if(acc != null)
            {
                return Ok(false);

            } else
            {
                auths.Add(authRequest);
                auths.TrimExcess();
                return Ok(true);
            }


        }
    }
}

