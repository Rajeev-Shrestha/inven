using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class DeliveryMethodViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Delivery Code is required")]
        [StringLength(10, ErrorMessage = "Delivery Code can be at most 10 characters.")]
        public string DeliveryCode { get; set; }

        [StringLength(200, ErrorMessage = "Description can be at most 200 characters.")]
        public string Description { get; set; }

        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
    }
}
