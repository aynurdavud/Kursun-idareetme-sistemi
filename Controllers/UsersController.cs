using Courseeee.Models;
using Courseeee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName,
                    Email = user.Email,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        public async Task<IActionResult> Activity(string? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }
            if (user.IsDeactive)
            {
                user.IsDeactive = false;
            }
            else
            {
                user.IsDeactive = true;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ChangeRole(string id)
        {
            {
                if (id == null)
                {
                    return View("Error");
                }
                AppUser user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return View("Error");
                }
                List<string> roles = new List<string>();
                roles.Add(Helpers.Roles.Admin.ToString());
                roles.Add(Helpers.Roles.Member.ToString());
                string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                ChangeRoleVM changeRole = new ChangeRoleVM
                {
                    Username = user.UserName,
                    Role = oldRole,
                    Roles = roles
                };

                return View(changeRole);
            }



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, string newRole)
        {

            if (id == null)
            {
                return View("Error");
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }
            List<string> roles = new List<string>();
            roles.Add(Helpers.Roles.Admin.ToString());
            roles.Add(Helpers.Roles.Member.ToString());
            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            ChangeRoleVM changeRole = new ChangeRoleVM
            {
                Username = user.UserName,
                Role = oldRole,
                Roles = roles
            };
            IdentityResult addIdentiyResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addIdentiyResult.Succeeded)
            {
                ModelState.AddModelError("", "Smoething is wrong!");
                return View(changeRole);
            }
            IdentityResult removeIdentiyResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
            if (!removeIdentiyResult.Succeeded)
            {
                ModelState.AddModelError("", "Smoething is wrong!");
                return View(changeRole);
            }



            return RedirectToAction("Index");
        }
    }
}

