using System;
using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class FiscalPeriod : BaseEntity
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public FiscalYear FiscalYear { get; set; }
        public Company Company { get; set; }

        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<JournalMaster> JournalMasters { get; set; }
    }
}
