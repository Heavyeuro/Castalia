using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Entities
{
    public class Teacher:BaseEntity
    {
      
        [MaxLength(30)]
        [Display(Name = "Teacher Of the course")]
        public string TeacherName { get; set; }

    }
}
