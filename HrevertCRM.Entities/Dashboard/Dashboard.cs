using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class Dashboard: BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int TotalOrders { get; set; }
        public int TotalSales { get; set; }
        public int TotalUser  { get; set; }
        public int TotalProduct { get; set; }
        public bool WebActive { get; set; }

        public virtual ICollection<SalesFiscalPeriodWise> SalesFiscalPeriodWise { get; set; }
        public virtual ICollection<OrderFiscalPeriodWise> OrderFiscalPeriodWise { get; set; }
    }

    public class OrderFiscalPeriodWise
    {
        public string FiscalPeriodName { get; set; }
        public int OrderCount { get; set; }
    }

    public class SalesFiscalPeriodWise
    {
        public string FiscalPeriodName { get; set; }
        public int SalesCount { get; set; }
    }
}
