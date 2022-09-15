using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Models
{
    public class Course
    {

        public int Id { get; set; }
        [Required]
        public string Ad { get; set; }
        [Required]
        public int Qrup_sayi { get; set; }
        
        public bool IsDeactive { get; set; }
        public string Description { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
