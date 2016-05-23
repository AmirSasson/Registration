using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

namespace HelloWebApi.Controllers
{

    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly TokenAuthOptions _tokenOptions;
        private readonly IRegistrationProvider _provider;



        public RegistrationController(ILogger<RegistrationController> logger, TokenAuthOptions tokenOptions, IRegistrationProvider provider)
        {
            _logger = logger;
            _tokenOptions = tokenOptions;
            _provider = provider;
        }


        // POST api/values
        [HttpPost]
        [Authorize("Bearer")]
        public dynamic Post([FromBody]UserDetails userDetails)
        {
            _logger.LogInformation("userDetails");

            int cid = _provider.Register(userDetails);

            return new { Cid = cid, LoginAccessToken = GetAccessToken(cid) };

        }

        private string GetAccessToken(int cid)
        {
            var handler = new JwtSecurityTokenHandler();

            // Here, you should create or look up an identity for the user which is being authenticated.
            // For now, just creating a simple generic identity.            
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(cid.ToString(), "TokenAuth"),
                new[] {
                    new Claim("EntityID", "1", ClaimValueTypes.Integer),
                    new Claim("Cid", cid.ToString(), ClaimValueTypes.Integer),

                });

            var securityToken = handler.CreateJwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                signingCredentials: _tokenOptions.SigningCredentials,
                subject: identity,
                expires: DateTime.Now.AddSeconds(90)
                );
            return handler.WriteToken(securityToken);
        }

    }
}
