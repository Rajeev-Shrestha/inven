using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Entities.Enumerations
{
    public class ImageTypes: BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [StringLength(20, ErrorMessage = "Value can be at most 20 characters.")]
        public string Value { get; set; }
    }
}
