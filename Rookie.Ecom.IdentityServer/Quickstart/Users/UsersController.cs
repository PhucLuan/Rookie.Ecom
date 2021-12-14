using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rookie.Ecom.IdentityServer.Data;
using Rookie.Ecom.IdentityServer.Models;
using Rookie.Ecom.IdentityServer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer.Quickstart.User
{
    [AllowAnonymous]
    public class UsersController : Controller
    {
        private readonly UserManager<Id4Users> _userManager;
        private readonly RoleManager<Id4Roles> _roleManager;
        private readonly SignInManager<Id4Users> _signInManager;
        private readonly Id4DbContext _context;
        private readonly IEmailSender _sender;

        //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
        public UsersController(
            Id4DbContext context,
            UserManager<Id4Users> userManager,
            RoleManager<Id4Roles> roleManager,
            SignInManager<Id4Users> signInManager,
            IEmailSender sender)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _sender = sender;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UsersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something wrong");
                return View(model);
            }
            Id4Users user = new Id4Users
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Contact = model.Contact,
                EmailConfirmed = false,
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                Id4Roles applicationRoles = await _roleManager.FindByNameAsync("User");
                if (applicationRoles != null)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRoles.Name);
                    
                    if (roleResult.Succeeded)
                    {
                        Address address = new Address();
                        address.UsersId = user.Id;
                        address.CustomerAddress = model.Address;
                        address.CreatedDate = DateTime.Now;
                        address.Published = true;
                        address.UpdatedDate = DateTime.Now;
                        _context.Addresses.Add(address);
                        _context.SaveChanges();

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var link = Url.Action(
                            nameof(VerifyEmail),
                            "Users",
                            new { userId = user.Id, code = code}, Request.Scheme, 
                            Request.Host.ToString());

                        await _sender.SendEmailAsync
                            (user.Email, 
                            "Xác nhận tài khoản", 
                            $"<a href=\"{link}\"> Click here to Verify Your Account </a>");

                        return RedirectToAction("EmailVerification");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)  return View();

            return BadRequest();
        }
        public IActionResult EmailVerification()
        {
            return View("EmailVerification");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callback = Url.Action(
                nameof(ResetPassword), 
                "Users", 
                new { token, email = user.Email }, Request.Scheme, Request.Host.ToString());

            await _sender.SendEmailAsync
                            (forgotPasswordModel.Email,
                            "Đổi mật khẩu",
                            $"<a href=\"{callback}\"> Click here to Verify Your Account </a>");
            //var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            //await _emailSender.SendEmailAsync(message);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
