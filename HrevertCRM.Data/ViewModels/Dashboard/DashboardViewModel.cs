using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels
{
    public class DashboardViewModel
    {
        public int? Id { get; set; }
        public int TotalOrders { get; set; }
        public int TotalSales { get; set; }
        public int TotalUser { get; set; }
        public int TotalProduct { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }

        public virtual ICollection<int> SalesMonthWise { get; set; }
        public virtual ICollection<int> OrderMonthWise { get; set; }
    }
}
