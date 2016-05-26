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
using HelloWebApi.Services;

namespace HelloWebApi.Controllers
{

    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly ITokenService _tokenService;
        private readonly IRegistrationProvider _registartionProvider;



        public RegistrationController(
            ILogger<RegistrationController> logger,
            IRegistrationProvider registrationProvider,
            ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _registartionProvider = registrationProvider;
        }


        // POST api/values
        [HttpPost]
        [Authorize("Bearer")]
        public dynamic Post([FromBody]UserDetails userDetails)
        {
            _logger.LogInformation("userDetails");

            int cid = _registartionProvider.Register(userDetails);

            return new { Cid = cid, LoginAccessToken = _tokenService.Issue(userDetails) };

        }



    }
}
