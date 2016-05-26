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
        IAuthenticationProvider _authenticationProviderr;
        public LoginController(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProviderr = authenticationProvider;
        }
        // GET: api/values
        [HttpGet]
        [Authorize("LoginScoped")]
        // GET api/values/5        
        public dynamic Get()
        {
            //string actualLoginToken = _authenticationProviderr.GetToken();

            return new { ActualLoginToken = "OK" };
        }
    }
}
