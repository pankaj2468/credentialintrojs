using credentialintrojs.Areas.Admin.Models;
using credentialintrojs.Areas.Admin.Models.Viewmodels;
using credentialintrojs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace credentialintrojs.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class LoggedController : Controller
    {
        private UserManager<IdentityCustomeUser> userManager;
        private readonly ApplicationDbContext context;
        private SignInManager<IdentityCustomeUser> signinManager;

        public LoggedController( ApplicationDbContext _context, SignInManager<IdentityCustomeUser> _signinManager, UserManager<IdentityCustomeUser> _userManager)
        {
            userManager = _userManager;
            signinManager = _signinManager;
            context = _context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {

            if (ModelState.IsValid)
            {
                var user = context.Users.SingleOrDefault(e => e.UserName == model.UserEmail);
                if (user != null)
                {
                    // var result=await signinManager.CheckPasswordSignInAsync(user, model.Password, false);
                    var result = await signinManager.PasswordSignInAsync(model.UserEmail, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new { @Areas = "Admin" });
                    }
                    else
                    {                        
                        ModelState.AddModelError(string.Empty, "Invalid login Credentials!");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "invalid user!");
                    return View();
                }

            }
            else
            {
                return View(model);
            }

        }

        public IActionResult SignUp()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Signup model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityCustomeUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    PasswordHash = model.Password,
                    PhoneNumber = model.Phone,
                    //Gender = model.Gender,

                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.message = "User Create Successfully !";
                    return View();
                }
                else
                {
                    foreach (var er in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, er.Description);
                    }
                    return View();
                }
            }
            else
            {
                return View(model);
            }

        }

    }
}
