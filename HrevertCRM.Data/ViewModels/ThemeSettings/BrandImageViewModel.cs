using HrevertCRM.Entities;
using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels.ThemeSettings
{
    public class BrandImageViewModel
    {
        public int? Id { get; set; }
        public List<ThemeImage> Images { get; set; }
        public int BrandId { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
