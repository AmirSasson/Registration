using System;
using HelloWebApi.Services;

namespace HelloWebApi.Controllers
{
    public class UserDetails : ICredentials
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
    }
}