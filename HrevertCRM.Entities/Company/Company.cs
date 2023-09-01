using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Company : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int? MasterId { get; set; }
        public string Name { get; set; }
        public string GpoBoxNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string VatRegistrationNo { get; set; }
        public string PanRegistrationNo { get; set; }
        public string WebsiteUrl { get; set; }
        public FiscalYearFormat FiscalYearFormat { get; set; }
        public bool WebActive { get; set; }
        public bool IsCompanyInitialized { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<FiscalYear> FiscalYears { get; set; }
        public virtual ICollection<SalesManager> SalesManagers { get; set; }
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<FiscalPeriod> FiscalPeriods { get; set; }
        public virtual ICollection<ProductPriceRule> ProductPriceRules { get; set; }

    }
}
