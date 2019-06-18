using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Castalia.WEB.Models
{
    public class LogViewModel
    {
        public List<Log> Logs { get; set; }

        /// <summary>
        /// All info about pagination
        /// </summary>
        public PagingInfo PagingInfo { get; set; }

        public string CourseStatus { get; set; }
    }
}