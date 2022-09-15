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
    public class TeachersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public TeachersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Teacher> dbteachers = await _db.Teachers.Include(x => x.course).ToListAsync();
            return View(dbteachers);
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Teacher dbteachers = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbteachers == null)
            {
                return View("Error");
            }
            if (dbteachers.IsDeactive)
            {
                dbteachers.IsDeactive = false;
            }
            else
            {
                dbteachers.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Course = await _db.Courses.Where(x => x.IsDeactive == false).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teachers, int courseId)
        {
            bool IsExist = _db.Teachers.Include(x => x.course).Any(x => x.Ad == teachers.Ad);
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
            await _db.Teachers.AddAsync(teachers);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Teacher teachers = await _db.Teachers.Include(x => x.course).FirstOrDefaultAsync(x => x.Id == id);
            if (teachers == null)
            {
                return View("Error");
            }
            ViewBag.Courses = await _db.Courses.Where(x => x.IsDeactive == false).ToListAsync();

            return View(teachers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Teacher teachers, int courseId)
        {
            if (id == null)
            {
                return View("Error");
            }
            Teacher dbteachers = await _db.Teachers.Include(x => x.course).FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Courses = await _db.Courses.Where(x => x.IsDeactive == false).ToListAsync();
            if (dbteachers == null)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }


            dbteachers.Ad = teachers.Ad;
            dbteachers.Soyad = teachers.Soyad;
            dbteachers.Tehsili = teachers.Tehsili;
            dbteachers.Tevellud = teachers.Tevellud;
            dbteachers.Elaqe_nomresi = teachers.Elaqe_nomresi;
            dbteachers.Email = teachers.Email;
            dbteachers.CourseId = courseId;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Teacher teachers = await _db.Teachers.Include(x => x.course).FirstOrDefaultAsync(x => x.Id == id);
            if (teachers == null)
            {
                return View("Error");
            }
            return View(teachers);

        }
    }
}
