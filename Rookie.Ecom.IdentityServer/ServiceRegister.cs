using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rookie.Ecom.IdentityServer.Data;
using Rookie.Ecom.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer
{
    public static class ServiceRegister
    {
        public static void AddIdentityserverAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Id4DbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<Id4Users, Id4Roles>()
                .AddEntityFrameworkStores<Id4DbContext>()
                .AddDefaultTokenProviders();

        }
    }
}
