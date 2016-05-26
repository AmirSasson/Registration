using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using HelloWebApi.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            //_tokenOptions = tokenOptions;
            _tokenService = tokenService;
        }


        // POST api/values
        [HttpPost]
        public dynamic Post([FromBody]Segment segment)
        {
            var token = _tokenService.Issue(segment);
            return new { authenticated = true, entityId = 1, token = token, tokenExpires = DateTime.Now.AddSeconds(90) };
        }




    }
}
