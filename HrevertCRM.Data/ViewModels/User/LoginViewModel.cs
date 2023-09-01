using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels.User
{

    public class LoginViewModel : LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public bool EnableLocalLogin { get; set; }
    }
    public class ExternalProvider
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}
