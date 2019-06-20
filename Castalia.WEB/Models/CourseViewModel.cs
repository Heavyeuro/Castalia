using Castalia.Domain.Entities;
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
        public DateTime StartDate { get; set; }//only in future


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

    public class CourseListViewModel
    {

        public List<Course> Courses { get; set; }
        public List<Teacher> Teachers { get; set; }

        /// <summary>
        /// All info about pagination
        /// </summary>
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// Current parameter that we are geting sample
        /// by
        /// </summary>
        public string CurrentParameter { get; set; }

        /// <summary>
        /// accordance courseId to posibility of student to register on course
        /// </summary>
        public Dictionary<int, bool> StudentRefisterPosibility { get; set; }


        /// <summary>
        /// accordance display name of the sorted parameter to its sorted id
        /// </summary>
        public Dictionary<string, string> SortedParam { get; set; }
    }


}