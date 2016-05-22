using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HelloWebApi
{
    internal class AttachedToUserRequirement : AuthorizationHandler<AttachedToUserRequirement>, IAuthorizationRequirement
    {
        protected override void Handle(AuthorizationContext context, AttachedToUserRequirement requirement)
        {
            var registered = context.User.HasClaim(c => c.Type == "Cid" && Convert.ToInt32(c.Value) > 0);

            if (registered)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}