using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Entities
{
    public class NicknameName:BaseEntity
    {

        public string UserName { get; set; }

        public Learner Learner { get; set; }

        public Teacher Teacher { get; set; }

    }
}
