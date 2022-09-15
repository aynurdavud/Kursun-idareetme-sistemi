using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Ad_Soyad { get; set; }
       
        [Required]
        public string Elaqe_nomresi { get; set; }

        public string Email { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
   

        public bool IsDeactive { get; set; }



    }
}
