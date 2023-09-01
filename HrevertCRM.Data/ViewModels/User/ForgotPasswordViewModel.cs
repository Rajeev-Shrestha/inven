using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels.User
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
