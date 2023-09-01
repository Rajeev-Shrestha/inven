using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels
{
    public class StageViewModel
    {
        public int? Id { get; set; }
        public string StageName { get; set; }
        public int Rank { get; set; }
        public int Probability { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }

        public List<SalesOpportunityViewModel> SalesOpportunities { get; set; }
    }
}
