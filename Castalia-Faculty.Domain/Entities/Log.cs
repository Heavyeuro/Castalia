using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Entities
{
    public class Log : BaseEntity
    {
        [Required]
        public Learner Lerner { get; set; }
        

        [Required]
        public Course  Course { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please, enter register date of the student")]
        [Display(Name = "Register date on the exact course")]
        public DateTime RegisterDate { get; set; }


        [Display(Name = "Mark(if empty student is studying")]
        public int? Mark { get; set; }
        

    }
}
