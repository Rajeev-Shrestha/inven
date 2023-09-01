using System.Collections.Generic;

namespace HrevertCRM.Data
{
    public class RetailerViewModel
    {
        public int? Id { get; set; }
        public int? DistibutorId { get; set; }
        public int? RetailerId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }

        public List<int> Retailers { get; set; }
        public List<int> Distributors { get; set; }
    }
}
