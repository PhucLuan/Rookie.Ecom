using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.CustomerSite.Controllers
{
    public class UsersController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return RedirectToAction("Index","Home");
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [HttpGet]
        public IActionResult SignUpRedirect()
        {
            
            return Redirect("https://localhost:5000/Users/SignUp");
        }
    }
}
