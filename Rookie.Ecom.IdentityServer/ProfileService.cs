using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Rookie.Ecom.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer
{
    public class ProfileService : IProfileService
    {
        protected UserManager<Id4Users> _userManager;
        public ProfileService(UserManager<Id4Users> userManager)
        {
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = await _userManager.GetUserAsync(context.Subject);
            var role = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim("FullName", user.Name),
                new Claim("role",role.FirstOrDefault().ToString())
            };

            context.IssuedClaims.AddRange(claims);
            context.IssuedClaims.AddRange(context.Subject.Claims);

            //return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}
