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

    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CoursesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Course> dbcourses = await _db.Courses.ToListAsync();
            return View(dbcourses);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Course courses = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (courses == null)
            {
                return View("Error");
            }

            return View(courses);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Course courses)
        {
            if (id == null)
            {
                return View("Error");
            }
            Course dbcourses = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbcourses == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }


            dbcourses.Ad = courses.Ad;
            dbcourses.Qrup_sayi = courses.Qrup_sayi;

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Course dbcourses = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbcourses == null)
            {
                return View("Error");
            }
            if (dbcourses.IsDeactive)
            {
                dbcourses.IsDeactive = false;
            }
            else
            {
                dbcourses.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course courses)
        {
            bool IsExist = _db.Courses.Any(x => x.Ad == courses.Ad);
            if (IsExist == true)
            {
                ModelState.AddModelError("Ad", "This category already IsExist");
                return View();
            }
            await _db.Courses.AddAsync(courses);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Course courses = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (courses == null)
            {
                return View("Error");
            }
            return View(courses);

        }
    }
}
