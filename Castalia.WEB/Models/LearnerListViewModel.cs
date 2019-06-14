using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Castalia.WEB.Models
{
    public class LearnerListViewModel
    {
        public IEnumerable<Learner> Learners { get; set; }

        /// <summary>
        /// All info about pagination
        /// </summary>
        public PagingInfo PagingInfo { get; set; }

    }
}