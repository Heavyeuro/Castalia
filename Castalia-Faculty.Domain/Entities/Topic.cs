using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Entities
{
    public class Topic : BaseEntity
    {
        [MaxLength(30)]
        [Display(Name = "Topic of the course")]
        [Required(ErrorMessage = "Please, enter name of the topic")]
        public string TopicName { get; set; }

    }
}
