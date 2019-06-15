using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Castalia.WEB.Models
{
    public class TeacherViewModel
    {
        [MaxLength(30)]
        [Display(Name = "Full name of the teacher")]
        public string TeacherName { get; set; }

        public Dictionary<string,bool> AvailableCourseName { get; set; }
    }
}