using System;

namespace HelloWebApi.Controllers
{
    public interface IActualLoginProvider
    {
        string GetToken();
    }

    public class Triple8LoginProvider : IActualLoginProvider
    {
        public string GetToken()
        {
            return Guid.NewGuid().ToString();
        }
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