using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.AuthServer.Models.AccountViewModels
{
    public class LoginViewModel : LoginInputModel
    {
        [Required]
        [EmailAddress]
     
        public bool EnableLocalLogin { get; set; }
    public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ExternalProvider
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}
