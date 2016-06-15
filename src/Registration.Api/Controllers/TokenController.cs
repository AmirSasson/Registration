using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using HelloWebApi.Services;
using HelloWebApi.Models;

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
        [Produces(typeof(SegmentToken))]        
        public dynamic Post([FromBody]Segment segment)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = _tokenService.Issue(segment);
            return new SegmentToken { Authenticated = true, EntityId = 1, Token = token, TokenExpires = DateTime.Now.AddSeconds(90) };
        }




    }
}
