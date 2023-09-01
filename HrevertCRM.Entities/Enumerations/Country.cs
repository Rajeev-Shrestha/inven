using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Entities.Enumerations
{
    public class Country:BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [StringLength(50, ErrorMessage = "Value can be at most 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }      
    }
}
