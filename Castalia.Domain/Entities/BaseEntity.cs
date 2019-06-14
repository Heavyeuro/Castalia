using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Entities
{
        /// <summary>
        /// Class BaseEntity allows auto inheritance of Id field
        /// </summary>
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

    }
}
