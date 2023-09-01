using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class SecurityViewModel
    {
        public int? Id { get; set; }

        public int SecurityCode { get; set; }

        [StringLength(100, ErrorMessage = "Security Description can be at most 100 characters.")]
        public string SecurityDescription { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public bool IsAssigned { get; set; }
    }
}
