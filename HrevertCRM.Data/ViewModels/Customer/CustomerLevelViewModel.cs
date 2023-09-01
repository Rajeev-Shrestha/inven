using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class CustomerLevelViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Customer Level Name is required")]
        [StringLength(50, ErrorMessage = "Customer Level Name can be at most 50 characters.")]
        public string Name { get; set; }

        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
    }
}
