using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HrevertCRM.Common;
using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Web.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(15, ErrorMessage = "First Name can be at most 15 characters.")]
        public string FirstName { get; set; }
        
        [StringLength(15, ErrorMessage = "Middle Name can be at most 15 characters.")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(15, ErrorMessage = "Last Name can be at most 15 characters.")]
        public string LastName { get; set; }

        public int Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100, ErrorMessage = "Address can be at most 100 characters.")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Phone number is required")]
        [StringLength(15, ErrorMessage = "Phone Number can be at most 15 characters.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email Address is required"), EmailAddress(ErrorMessage = "Email Address should be in proper format.")]
        [StringLength(100, ErrorMessage = "Email can be at most 100 characters.")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "UserName can be at most 100 characters.")]
        public string UserName { get; set; }

      //  [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password must be between {2} to {1} characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("Re-type Password")]
      //[Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Re-type Password must be between {2} to {1} characters.", MinimumLength = 6)]
       // [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        //[StringLength(50, ErrorMessage = "Company Name can be at most 50 characters.")]
        //public string CompanyName { get; set; }

        //[EnumDataType(typeof(UserType))]
        public UserType UserType { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; } = true;
        public byte[] Version { get; set; }
        public bool WebActive { get; set; }

        public List<int> SecurityGroups { get; set; }
        public List<int> SecurityGroupIdList { get; set; }
        public bool IsCompanyInitialized { get; set; }
        public bool IsEstoreInitialized { get; set; }
        public byte[] CompanyVersion { get; set; }
        public string CompanyName { get; set; }
        public virtual CompanyViewModel CompanyViewModel { get; set; }
        public bool HasAuthorityToAssignRight { get; set; }
    }
}
