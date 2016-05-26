using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWebApi.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        IAuthenticationProvider _loginProvider;
        public LoginController(IAuthenticationProvider loginProvider)
        {
            _loginProvider = loginProvider;
        }   
        // GET: api/values
        [HttpGet]
        [Authorize("AttachedToUser")]
        // GET api/values/5        
        public dynamic Get()
        {
            string actualLoginToken = _loginProvider.GetToken();

            return new { ActualLoginToken = actualLoginToken };
        }
    }
}
