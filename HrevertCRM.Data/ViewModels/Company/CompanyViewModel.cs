using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class CompanyViewModel : IWebItem
    {
        public int? Id { get; set; }
        public int? MasterId { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(100, ErrorMessage = "Company Name can be at most 100 characters.")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "GPO Box Address can be at most 20 characters.")]
        public string GpoBoxNumber { get; set; }

        [Required(ErrorMessage = "Company Address is required")]
        [StringLength(100, ErrorMessage = "Company Address can be at most 200 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Company Phone Number is required")]
        [StringLength(100, ErrorMessage = "Company Name can be at most 20 characters.")]
        public string PhoneNumber { get; set; }

        [StringLength(20, ErrorMessage = "Company Name can be at most 20 characters.")]
        public string FaxNo { get; set; }

        [Required(ErrorMessage = "Company Email Address is required")]
        [StringLength(100, ErrorMessage = "Company Email Address can be at most 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Company Website Url is required")]
        [StringLength(100, ErrorMessage = "Company WebsiteUrl can be at most 50 characters.")]
        public string WebsiteUrl { get; set; }

        [StringLength(100, ErrorMessage = "Company VAT Number can be at most 15 characters.")]
        public string VatRegistrationNo { get; set; }

        [StringLength(100, ErrorMessage = "Company PAN Number can be at most 15 characters.")]
        public string PanRegistrationNo { get; set; }

        public FiscalYearFormat FiscalYearFormat { get; set; }
        public bool IsCompanyInitialized { get; set; }

        public bool WebActive { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}