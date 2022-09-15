using Courseeee.DAL;
using Courseeee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickMailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public StudentsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Student> dbstudent = await _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).ToListAsync();

            return View(dbstudent);
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Student dbstudent = await _db.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (dbstudent == null)
            {
                return View("Error");
            }
            if (dbstudent.IsDeactive)
            {
                dbstudent.IsDeactive = false;
            }
            else
            {
                dbstudent.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Create()
        {

            ViewBag.Groups = await _db.Groups.ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student students, int[] groupId)
        {

            bool IsExist = _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).Any(x => x.Ad_Soyad == students.Ad_Soyad);
            ViewBag.Groups = await _db.Groups.Where(x => x.IsDeactive == false).ToListAsync();

            if (!ModelState.IsValid)
            {

                return View();
            };
            if (IsExist == true)
            {
                ModelState.AddModelError("Ad", "This Name already IsExist");
                return View();
            }

            List<StudentGroup> studentGroups = new List<StudentGroup>();
            foreach (int item in groupId)
            {
                StudentGroup studentGroup = new StudentGroup
                {
                    GroupId = item,
                };
                studentGroups.Add(studentGroup);
            }
            students.StudentGroups = studentGroups;
            await _db.Students.AddAsync(students);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student dbstudent = await _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x=>x.Id==id);
            if (dbstudent == null)
            {
                return NotFound();
            }
            ViewBag.Groups = await _db.Groups.Where(x => x.IsDeactive == false).ToListAsync();

            return View(dbstudent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Student student,int[] groupId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student dbstudent = await _db.Students.Include(x => x.StudentGroups).ThenInclude(x => x.Group).FirstOrDefaultAsync(x=>x.Id==id);
            if (dbstudent == null)
            {
                return NotFound();
            }
            ViewBag.Groups = await _db.Groups.Where(x => x.IsDeactive == false).ToListAsync();

            if (!ModelState.IsValid)
            {

                return View();
            };
           

            List<StudentGroup> studentGroups = new List<StudentGroup>();
            foreach (int item in groupId)
            {
                StudentGroup studentGroup = new StudentGroup
                {
                    GroupId = item,
                };
                studentGroups.Add(studentGroup);
            }
            dbstudent.StudentGroups = studentGroups;
            dbstudent.Ad_Soyad = student.Ad_Soyad;
            dbstudent.Elaqe_nomresi = student.Elaqe_nomresi;
            dbstudent.Email = student.Email;
       
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       
    }
}
