using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Rookie.Ecom.DataAccessor.Data;
using Rookie.Ecom.DataAccessor.Models;

namespace Rookie.Ecom.DataAccessor.Code
{
    public class IdentityDbInitializer
    {
        public static ApplicationDbContext _context;

        public IdentityDbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public static void SeedData(UserManager<ApplicationUsers> userManager,
            RoleManager<ApplicationRoles> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ApplicationUsers> userManager)
        {
            if (userManager.FindByNameAsync
                    ("user1").Result == null)
            {
                ApplicationUsers user = new ApplicationUsers
                {
                    UserName = "LuanNguyen",
                    Email = "phucluan6052000@gmail.com",
                    Name = "Phuc Luan",
                    DateOfBirth = new DateTime(2000, 10, 10),
                };

                IdentityResult result = userManager.CreateAsync
                    (user, "12345678").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                        "User").Wait();
                }
            }


            if (userManager.FindByNameAsync
                    ("admin").Result == null)
            {
                ApplicationUsers user = new ApplicationUsers
                {
                    UserName = "admin",
                    Email = "admin@localhost.com",
                    Name = "Nguyen Phuc Luan",
                    DateOfBirth = new DateTime(2000, 7, 6),
                };

                IdentityResult result = userManager.CreateAsync
                    (user, "12345678").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                        "Admin").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<ApplicationRoles> roleManager)
        {
            if (!roleManager.RoleExistsAsync
                ("User").Result)
            {
                ApplicationRoles role = new ApplicationRoles
                {
                    Name = "User",
                    Description = "Perform normal operations."
                };
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
                ("Admin").Result)
            {
                ApplicationRoles role = new ApplicationRoles
                {
                    Name = "Admin",
                    Description = "Toàn quyền trên hệ thống"
                };
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync
                ("Editor").Result)
            {
                ApplicationRoles role = new ApplicationRoles
                {
                    Name = "Editor",
                    Description = "Quyền sửa đổi thông tin"
                };
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }
        }
    }
}
