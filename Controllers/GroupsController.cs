using Courseeee.DAL;
using Courseeee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public GroupsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Group> dbgroups = await _db.Groups.Include(x => x.teacher).ThenInclude(x => x.course).ToListAsync();
            return View(dbgroups);
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Group dbgroups = await _db.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbgroups == null)
            {
                return View("Error");
            }
            if (dbgroups.IsDeactive)
            {
                dbgroups.IsDeactive = false;
            }
            else
            {
                dbgroups.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Teacher = await _db.Teachers.Where(x => x.IsDeactive == false).ToListAsync();
            ViewBag.Course = await _db.Courses.Where(x => x.IsDeactive == false).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group groups, int courseId, int teacherId)
        {
            bool IsExist = _db.Groups.Include(x => x.teacher).ThenInclude(x => x.course).Any(x => x.Ad == groups.Ad);
            ViewBag.Teacher = await _db.Teachers.Where(x => x.IsDeactive == false).ToListAsync();
            ViewBag.Course = await _db.Courses.Where(x => x.IsDeactive == false).ToListAsync();
            if (!ModelState.IsValid)
            {

                return View();
            };
            if (IsExist == true)
            {
                ModelState.AddModelError("Ad", "This Name already IsExist");
                return View();
            }
            await _db.Groups.AddAsync(groups);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Group groups = await _db.Groups.Include(x => x.teacher).ThenInclude(x => x.course).FirstOrDefaultAsync(x => x.Id == id);
            if (groups == null)
            {
                return View("Error");
            }
            ViewBag.Teachers = await _db.Teachers.Where(x => x.IsDeactive == false).ToListAsync();

            ViewBag.Courses = await _db.Courses.Where(x => x.IsDeactive == false).ToListAsync();

            return View(groups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Group groups, int courseId, int teacherId)
        {
            if (id == null)
            {
                return View("Error");
            }
            Group dbgroups = await _db.Groups.Include(x => x.teacher).ThenInclude(x => x.course).FirstOrDefaultAsync(x => x.Id == id);

            ViewBag.Teachers = await _db.Teachers.Where(x => x.IsDeactive == false).ToListAsync();

            ViewBag.Courses = await _db.Courses.Where(x => x.IsDeactive == false).ToListAsync();
            if (dbgroups == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }


            dbgroups.Ad = groups.Ad;
            dbgroups.Vaxt = groups.Vaxt;
            dbgroups.Telebe_sayi = groups.Telebe_sayi;
            dbgroups.TeacherId = groups.TeacherId;

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}
