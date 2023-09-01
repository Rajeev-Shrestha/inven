using Hrevert.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.ViewModels
{
    public class FeaturedItemMetadataViewModel
    {
        public int ? Id { get; set; }

        public int FeaturedItemId { get; set; }

        public int ItemId { get; set; }
        [EnumDataType(typeof(MediaType))]
        public MediaType MediaType { get; set; }

        [Required(ErrorMessage = "Media Url is required")]
        [StringLength(500, ErrorMessage = "Media Url can be at most 500 characters.")]
        public string MediaUrl { get; set; }

        public ImageType  ImageType { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
    }
}
