using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels
{
    public class PayMethodInPayTermViewModel
    {
        public int? Id { get; set; }
        public int PayTermId { get; set; }
        public List<int> PayMethodIds { get; set; }
        public bool WebActive { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
    }
}
