using Courseeee.Helpers;
using Courseeee.Models;
using Courseeee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Courseeee.Helpers.Extension;

namespace Courseeee.Controllers
{
    
    
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signManager = signManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task CreateRoles()
        {
            if (!(await _roleManager.RoleExistsAsync(Roles.Admin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString()});
            }
            else if (!(await _roleManager.RoleExistsAsync(Roles.Member.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString()});
            }
            
        }
        public IActionResult Register()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return View("Error");
            //}
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser newUser = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.Username,

            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            return RedirectToAction("Index", "Users");

        }
        public async Task<IActionResult> Logout()
        {

            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Groups");
        }
        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or Password wrong!");
                return View();
            }
            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("", "Your accaunt has been blocked!");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your accaunt locked out 1min!");

                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or  Password wrong!");
                return View();
            }
            return RedirectToAction("Index", "Courses");
        }
    }
}
