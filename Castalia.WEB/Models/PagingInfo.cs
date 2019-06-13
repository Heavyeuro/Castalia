using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Castalia.WEB.Models
{
    public class PagingInfo
    {
        // Total amount of items
        public int TotalItems { get; set; }

        // Maximum of items on every page
        public int ItemsPerPage { get; set; }

        // Number of curret page
        public int CurrentPage { get; set; }

        // Total amount of pages
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}