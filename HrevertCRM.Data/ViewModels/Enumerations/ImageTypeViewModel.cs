using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.ViewModels.Enumerations
{
    public class ImageTypeViewModel
    {
        public int? Id { get; set; }
        public string Value { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
