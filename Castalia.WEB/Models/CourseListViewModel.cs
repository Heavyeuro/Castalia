using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Castalia.WEB.Models
{
    public class CourseListViewModel
    {

        public IEnumerable<Course> Courses { get; set; }

        /// <summary>
        /// All info about pagination
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
       
            /// <summary>
            /// Current parameter that we are geting sample
            /// by
            /// </summary>
        public string CurrentParameter { get; set; }

        public Dictionary<string,string> SortedParam { get; set; } 
    }
}