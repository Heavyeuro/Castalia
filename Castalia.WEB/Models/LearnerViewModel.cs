using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Castalia.WEB.Models
{
    public class LearnerViewModel
    {
        public List<Log> logs { get; set; }

        public string CurrentCourse { get; set; }

        public List<string> CoursesList { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}