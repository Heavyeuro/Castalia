using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Castalia.WEB.Models
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, enter full name of the course")]
        [MaxLength(30)]
        [Display(Name = "Full name of the course")]
        public string CourseName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please, enter start date of the course")]
        [Display(Name = "Start date of the course")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please, enter duration of the course")]
        [Range (1,366,ErrorMessage="Range Must be between 1 to 365")]
        [Display(Name = "Duration of course (days)")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Please, enter topic of the course")]
        public string Topic { get; set; }

        [Required]
        [Display(Name = "Amount of student that registred")]
        public int AmountOfStudents { get; set; }
    }
}