using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class CustomerContactGroupViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Group Name is required")]
        [StringLength(50, ErrorMessage = "Group Name can be at most 50 characters..")]
        public string GroupName { get; set; }

        [StringLength(200, ErrorMessage = "Group Description can be at most 200 characters.")]
        public string Description { get; set; }


        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }

        public List<CustomerInContactGroupViewModel> CustomerAndContactGroupList { get; set; }
        public List<int> CustomerIdsInContactGroup { get; set; }
    }
}
