using System;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class JournalMaster : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public JournalType JournalType { get; set; } // General, Purchase, Payment, Sales, Receipt
        public bool Closed { get; set; }
        public string Description { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public int FiscalPeriodId { get; set; }
        public bool Posted { get; set; }
        public string Note { get; set; }
        public DateTime PostedDate { get; set; }
        public bool Printed { get; set; }
        public bool Cancelled { get; set; }
        public bool IsSystem { get; set; } // Indicated journal can't be modified
        public bool WebActive { get; set; }

        public FiscalPeriod FiscalPeriod { get; set; }
    }
}
