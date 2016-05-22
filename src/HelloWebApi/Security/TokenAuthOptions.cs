using Microsoft.IdentityModel.Tokens;

namespace HelloWebApi
{
    public class TokenAuthOptions
    {
        public TokenAuthOptions()
        {
        }

        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}