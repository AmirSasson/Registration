using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HelloWebApi.Controllers;

namespace HelloWebApi
{
    public class Startup
    {
        RsaSecurityKey _key;
        TokenAuthOptions _tokenOptions;
        public Startup(IHostingEnvironment env)
        {
            // System.IdentityModel.Tokens.Jwt.JwtSecurityToken t = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(new JwtHeader(), new JwtPayload());
            //            t.Payload.Add("Cid", 42342342);

            //          string s = new JwtSecurityTokenHandler().WriteToken(t);

            //t.ToString

            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();



            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //RSAParameters keyParams = RSAKeyUtils.GetRandomKey();

            // Create the key, and a set of token options to record signing credentials 
            // using that key, along with the other parameters we will need in the 
            // token controlller.
            RSAParameters keyParams = RSAKeyUtils.GetRandomKey();
            _key = new RsaSecurityKey(keyParams);

            _tokenOptions = new TokenAuthOptions()
            {
                Audience = "Myself",
                Issuer = "MMBR",
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256Signature/*, SecurityAlgorithms.Sha256Digest*/)
            };

            // Save the token options into an instance so they're accessible to the 
            // controller.
            services.AddSingleton(_tokenOptions);
            services.AddTransient<IAuthenticationProvider, Triple8LoginProvider>();
            services.AddTransient<IRegistrationProvider, Triple8RegisterProvider>();

            // Enable the use of an [Authorize("Bearer")] attribute on methods and
            // classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
                auth.AddPolicy("AttachedToUser", policy => policy.Requirements.Add(new AttachedToUserRequirement()));
                //auth.AddPolicy("Access", new AuthorizationPolicy(new[] { new Over18Requirement() }, new[] { JwtBearerDefaults.AuthenticationScheme }));

            });


            //auth.AddPolicy("Access", policy =>
            //{
            //    policy.AuthenticationSchemes.Add("Bearer");
            //    policy.RequireAuthenticatedUser();
            //    policy.Requirements.Add(new Over18Requirement())
            //        });


            // Add framework services.
            services.AddMvc();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            var logger = loggerFactory.CreateLogger("Responses");

            //app.UseA
            //app.UseJwtBasicAuthentication()


            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                TokenValidationParameters = new TokenValidationParameters()
                {
                    // Basic settings - signing key to validate with, audience and issuer.
                    IssuerSigningKey = _key,
                    ValidAudience = _tokenOptions.Audience,
                    ValidIssuer = _tokenOptions.Issuer,

                    //ValidateIssuerSigningKey = true,
                     
                    // When receiving a token, check that we've signed it.
                    //ValidateSignature = true,
                    //SignatureValidator = CustomSignatureValidator,
                    //SignatureValidator=new 
                    // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,

                    // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                    // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                    // machines which should have synchronised time, this can be set to zero. Where external tokens are
                    // used, some leeway here could be useful.
                    ClockSkew = TimeSpan.FromMinutes(0)
                }


            });

            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                logger.LogInformation("Executing...");

                await next();
                logger.LogInformation("Finished...");
            });

            app.UseMetricsMiddleware();
            app.UseMvc();

        }

        public static SecurityToken CustomSignatureValidator(string token, TokenValidationParameters validationParameters)
        {
            var t = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return t;
        }
    }
}
