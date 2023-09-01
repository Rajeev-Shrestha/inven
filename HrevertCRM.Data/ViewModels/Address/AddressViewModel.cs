using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class AddressViewModel
    {
        public int? Id { get; set; }
        public int CountryId { get; set; }
        public string UserId { get; set; }

        [EnumDataType(typeof(TitleType))]
        public TitleType Title { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(15, ErrorMessage = "First Name can be at most 15 characters.")]
        public string FirstName { get; set; }

        [StringLength(15, ErrorMessage = "Middle Name can be at most 15 characters.")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(15, ErrorMessage = "Last Name can be at most 15 characters.")]
        public string LastName { get; set; }

        [EnumDataType(typeof(SuffixType))]
        public SuffixType Suffix { get; set; }

        [Required(ErrorMessage = "Address Line is required")]
        [StringLength(100, ErrorMessage = "Address Line can be at most 100 characters.")]
        public string AddressLine1 { get; set; }

        [StringLength(100, ErrorMessage = "Address Line can be at most 100 characters.")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City Name is required")]
        [StringLength(30, ErrorMessage = "City Name can be at most 30 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State Name is required")]
        [StringLength(30, ErrorMessage = "State Name can be at most 30 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(10, ErrorMessage = "Zip Code can be at most 30 characters.")]
        public string ZipCode { get; set; }
        
        [StringLength(20, ErrorMessage = "Telephone Number can be at most 20 characters.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Mobile Phone number is required")]
        [StringLength(20, ErrorMessage = "Mobile Phone can be at most 20 characters.")]
        public string MobilePhone { get; set; }

        //[Required(ErrorMessage = "Fax is required")]
        [StringLength(20, ErrorMessage = "Fax can be at most 20 characters.")]
        public string Fax { get; set; }

        [Required(ErrorMessage = "Email address is required"), EmailAddress(ErrorMessage = "Email address should be in proper format")]
        [StringLength(50, ErrorMessage = "Email can be at most 50 characters.")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Website Name can be at most 50 characters.")]
        [DataType(DataType.Url)]
        [UIHint("OpenInNewWindow")]
        public string Website { get; set; }

        [EnumDataType(typeof(AddressType))]
        public AddressType AddressType { get; set; }


        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public bool IsDefault { get; set; }
        public int? CustomerId { get; set; }
        public int? VendorId { get; set; }
        public int? DeliveryZoneId { get; set; }

    }
}
