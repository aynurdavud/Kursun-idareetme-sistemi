using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        public string Ad { get; set; }
        [Required]
        public string Vaxt { get; set; }
        [Required]
        public int Telebe_sayi { get; set; }
        public Teacher teacher { get; set; }
        
        public int TeacherId { get; set; }
        public bool IsDeactive { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }

      
    }
}
