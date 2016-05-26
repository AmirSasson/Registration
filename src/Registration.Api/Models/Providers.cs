using System;
using HelloWebApi.Services;

namespace HelloWebApi.Controllers
{
    public interface IAuthenticationProvider
    {
        //string GetToken();
        bool Authenticate(ICredentials credentials);
    }

    public class Triple8LoginProvider : IAuthenticationProvider
    {
        public bool Authenticate(ICredentials credentials)
        {
            return true;
        }

        //public string GetToken()
        //{
        //    return Guid.NewGuid().ToString();
        //}
    }


    public interface IRegistrationProvider
    {
        int Register(UserDetails userDetails);
    }

    public class Triple8RegisterProvider : IRegistrationProvider
    {
        public int Register(UserDetails userDetails)
        {
            return new Random((int)DateTime.Now.Ticks).Next();
        }
    }
}