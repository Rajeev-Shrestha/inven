using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels.User
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
