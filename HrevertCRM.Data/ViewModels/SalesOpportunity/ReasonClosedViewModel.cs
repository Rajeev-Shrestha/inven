using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class ReasonClosedViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Reason Line is required")]
        public string Reason { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
    }
}
