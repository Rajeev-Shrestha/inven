using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class EmailSettingViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Host name is required")]
        [StringLength(255, ErrorMessage = "Host can be at most 255 characters.")]
        public string Host { get; set; }

        [Required(ErrorMessage = "Port number is required")]
        public int Port { get; set; }

        [Required(ErrorMessage = "Sender Name is required")]
        [StringLength(25, ErrorMessage = "Sender Name can be at most 50 characters.")]
        public string Sender { get; set; }

        [Required(ErrorMessage = "User name is required"), EmailAddress(ErrorMessage = "Email Address should be in proper format.")]
        [StringLength(50, ErrorMessage = "Username can be at most 50 characters.")]
        public string UserName { get; set; }

       // [Required(ErrorMessage = "Password is required")]
       // [StringLength(30, ErrorMessage = "Password must be between {2} to {1} characters.", MinimumLength = 6)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //[DisplayName("Re-type Password")]
        //[StringLength(30, ErrorMessage = "Password must be between {2} to {1} characters.", MinimumLength = 6)]
        //[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(30, ErrorMessage = "Name can be at most 30 characters.")]
        public string Name { get; set; }

        [EnumDataType(typeof(EncryptionType))]
        public EncryptionType EncryptionType { get; set; }

    //  [Range(typeof(bool), "true", "false", ErrorMessage = "The field Require Authentication must be checked.")]
        public bool RequireAuthentication { get; set; }

        public bool WebActive { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
    }
}
