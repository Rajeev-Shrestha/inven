using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Entities.Enumerations
{
    public class EntryMethodTypes:BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [StringLength(50, ErrorMessage = "Value can be at most 50 characters.")]
        public string Value { get; set; }
    }
}
