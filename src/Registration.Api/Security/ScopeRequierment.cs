using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;

namespace HelloWebApi
{
    internal class ScopeRequierment : AuthorizationHandler<ScopeRequierment>, IAuthorizationRequirement
    {
        IEnumerable<string> _requiredScopes;
        public ScopeRequierment(IEnumerable<string> scopes)
        {
            _requiredScopes = scopes;
        }
        protected override void Handle(AuthorizationContext context, ScopeRequierment requirement)
        {
            var tokenScope = context.User.FindFirst("Scope");
            string[] tokenScopes = tokenScope.Value.Split(' ');

            foreach (var requiredScope in _requiredScopes)
            {
                if (!tokenScopes.Contains(requiredScope))//case sensative...
                {
                    context.Fail();
                    return;
                }
            }
            context.Succeed(requirement);
        }
    }
}