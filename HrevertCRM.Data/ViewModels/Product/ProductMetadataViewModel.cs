using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class ProductMetadataViewModel : IWebItem
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }

        [EnumDataType(typeof(MediaType))]
        public MediaType MediaType { get; set; }

        [Required(ErrorMessage = "Media Url is required")]
        [StringLength(500, ErrorMessage = "Media Url can be at most 500 characters.")]
        public string MediaUrl { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
    }
}
