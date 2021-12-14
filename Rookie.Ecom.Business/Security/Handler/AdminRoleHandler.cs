using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Rookie.Ecom.Business.Security.Requirement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Security.Handler
{
    public class AdminRoleHandler : AuthorizationHandler<AdminRoleRequirement>
    {
        private readonly IConfiguration _configuration;
        public AdminRoleHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       AdminRoleRequirement requirement)
        {
            var claims = context.User.Claims.ToList();
            var adminRole = claims.FirstOrDefault(c => c.Type.Contains("role") && c.Issuer == "https://localhost:5000" &&
                                                     c.Value.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))?.Value;

            if (!string.IsNullOrEmpty(adminRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
