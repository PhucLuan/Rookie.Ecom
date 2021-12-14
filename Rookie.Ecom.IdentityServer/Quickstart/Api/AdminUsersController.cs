using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.IdentityServer.Data;
using Rookie.Ecom.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer.Quickstart.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly UserManager<Id4Users> _userManager;
        private readonly RoleManager<Id4Roles> _roleManager;
        private readonly SignInManager<Id4Users> _signInManager;
        private readonly Id4DbContext _context;

        public AdminUsersController(UserManager<Id4Users> userManager,
           RoleManager<Id4Roles> roleManager,
           SignInManager<Id4Users> signInManager, Id4DbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Authorize("ApiScope")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync(string search = "")
        {
            var model = new List<UserListDto>();
            model = await _userManager.Users.Take(1000).Include(x => x.Addresses).Select(u =>
           new UserListDto
           {
               Id = u.Id,
               Name = u.Name,
               Contact = u.Contact,
               Gender = u.Gender,
               Email = u.Email,
               Address = u.Addresses.FirstOrDefault().CustomerAddress
           }).ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                model.Where(x =>
                   x.Name.ToLower().Contains(search.ToLower()) ||
                   x.Email.ToLower().Contains(search.ToLower()) ||
                   x.Contact.ToLower().Contains(search.ToLower())
                );
            }

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet("UsersComment")]
        public async Task<IEnumerable<UserListDto>> GetUsersCommentAsync([FromBody]List<string> userids)
        {
            IEnumerable<UserListDto> model = new List<UserListDto>();
            model = await _userManager.Users.Where(x => userids.Contains(x.Id)).Select(u =>
               new UserListDto
               {
                   Id = u.Id,
                   Name = u.Name,
                   Contact = u.Contact,
                   Gender = u.Gender,
                   Email = u.Email
               }).ToListAsync();

            return model;
        }

        [HttpGet("UsersAddress/{userid}")]
        public async Task<IEnumerable<UserAddressDto>> GetUsersAddressAsync(string userid)
        {
            return await _context.Addresses
                .Where(x => x.UsersId == userid)
                .Select(x => new UserAddressDto {
                    UsersId = x.UsersId,
                    CustomerAddress = x.CustomerAddress,
                    AddressID = x.Id.ToString()
                } ).ToListAsync();
        }

        [HttpGet("UsersInfo")]
        public async Task<IEnumerable<UserListDto>> GetUsersInfoAsync([FromBody] List<string> userids)
        {
            IEnumerable<UserListDto> model = new List<UserListDto>();
            model = await _userManager.Users
                .Include(x => x.Addresses)
                .Where(x => userids.Contains(x.Id)).Select(u =>
               new UserListDto
               {
                   Id = u.Id,
                   Name = u.Name,
                   Contact = u.Contact,
                   Gender = u.Gender,
                   Email = u.Email,
                   Address = u.Addresses.FirstOrDefault().CustomerAddress
               }).ToListAsync();

            return model;
        }

        [HttpGet("GetOrderUserInfo/{userid}/{addressid}")]
        public async Task<IEnumerable<UserListDto>> GetOrderUserInfoAsync(string userid, Guid addressid)
        {
            IEnumerable<UserListDto> model = new List<UserListDto>();
            model = await _userManager.Users
                .Include(x => x.Addresses)
                .Where(x => x.Id == userid).Select(u =>
               new UserListDto
               {
                   Id = u.Id,
                   Name = u.Name,
                   Contact = u.Contact,
                   Gender = u.Gender,
                   Email = u.Email,
                   Address = u.Addresses.FirstOrDefault(a => a.Id == addressid).CustomerAddress
               }).ToListAsync();

            return model;
        }
    }
}
