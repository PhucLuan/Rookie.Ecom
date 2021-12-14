using Microsoft.AspNetCore.Identity;
using Rookie.Ecom.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer.Data
{
    public class IdentityDbInitializer
    {
        public static Id4DbContext _context;

        public IdentityDbInitializer(Id4DbContext context)
        {
            _context = context;
        }

        public static void SeedData(UserManager<Id4Users> userManager,
            RoleManager<Id4Roles> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<Id4Users> userManager)
        {
            if (userManager.FindByNameAsync
                    ("user1").Result == null)
            {
                Id4Users user = new Id4Users
                {
                    UserName = "Id4LuanNguyen",
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
                Id4Users user = new Id4Users
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

        public static void SeedRoles(RoleManager<Id4Roles> roleManager)
        {
            if (!roleManager.RoleExistsAsync
                ("User").Result)
            {
                Id4Roles role = new Id4Roles
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
                Id4Roles role = new Id4Roles
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
                Id4Roles role = new Id4Roles
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
