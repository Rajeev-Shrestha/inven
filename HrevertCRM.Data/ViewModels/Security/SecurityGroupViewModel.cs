using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class SecurityGroupViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Security Group Name is required")]
        [StringLength(20, ErrorMessage = "Group Name can be at most 20 characters.")]
        public string GroupName { get; set; }

        [StringLength(100, ErrorMessage = "Group Description can be at most 100 characters.")]
        public string GroupDescription { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
    }
}
