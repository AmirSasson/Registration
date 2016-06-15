using HelloWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HelloWebApi.Services
{
    public interface ITokenService
    {
        string Issue(ISegment segment);

        string Issue(ICredentials credentials);

    }

    public interface ICredentials
    {
        string Username { get; }
        string Password { get; }
    }

    public class Segment : ISegment
    {
        [Range(0, 100)]
        [Required(AllowEmptyStrings = false)]
        public int Brand { get; set; }
        [Required]
        public int SubBrand { get; set; }
    }

    public class TokenService : ITokenService
    {
        TokenAuthOptions _tokenOptions;
        IAuthenticationProvider _authenticationProvider;
        public TokenService(TokenAuthOptions tokenOptions, IAuthenticationProvider authenticationProvider)
        {
            _tokenOptions = tokenOptions;
            _authenticationProvider = authenticationProvider;
        }
        public string Issue(ISegment segment)
        {
            var handler = new JwtSecurityTokenHandler();

            // Here, you should create or look up an identity for the user which is being authenticated.
            // For now, just creating a simple generic identity.
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity("1", "TokenAuth"),
                new[] {
                    new Claim("EntityID", "1", ClaimValueTypes.Integer),
                    new Claim("Brand", segment.Brand.ToString(), ClaimValueTypes.Integer),
                    new Claim("SubBrand", segment.SubBrand.ToString(), ClaimValueTypes.Integer),
                    new Claim("CountryOfIp", "Georgia", ClaimValueTypes.String),
                    new Claim("Scope", "Registration ForgotPassword", ClaimValueTypes.String),

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

        public string Issue(ICredentials credentials)
        {
            var handler = new JwtSecurityTokenHandler();

            if (_authenticationProvider.Authenticate(credentials))
            {
                // Here, you should create or look up an identity for the user which is being authenticated.
                // For now, just creating a simple generic identity.            
                ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(credentials.Username.ToString(), "TokenAuth"),
                    new[] {
                    new Claim("EntityID", "1", ClaimValueTypes.Integer),
                    new Claim("Username", credentials.Username, ClaimValueTypes.String),
                    new Claim("Scope", "Login Play", ClaimValueTypes.String),

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
            return null;

        }
    }
}
