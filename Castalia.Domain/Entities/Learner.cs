using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Entities
{
    public class Learner : BaseEntity
    {
        [MaxLength(30)]
        [Required(ErrorMessage = "Please, enter name of the learner")]
        [Display(Name = "Student Name")]
        public string LearnerName { get; set; }

        [Required]
        public bool IsBlocked { get; set; }
    }
}
