using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        public string Ad { get; set; }
        [Required]
        public string Soyad { get; set; }
        public DateTime Tevellud { get; set; }
        [Required]
        public float Elaqe_nomresi { get; set; }

        public string Email { get; set; }
        [Required]
        public string Tehsili { get; set; }
        public Course course { get; set; }
        public int CourseId { get; set; }
        public List<Group> Groups { get; set; }
        public bool IsDeactive { get; set; }
    }
}
