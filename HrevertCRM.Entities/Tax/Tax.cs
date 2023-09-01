using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Tax : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string TaxCode { get; set; }
        public string Description { get; set; }
        public bool IsRecoverable { get; set; }
        public TaxCaculationType TaxType { get; set; }
        public decimal TaxRate { get; set; }
        public int RecoverableCalculationType { get; set; } //May be straight forward things like VAT calulation type
        public bool WebActive { get; set; }


        public ICollection<TaxesInProduct> TaxesInProducts { get; set; }
        public ICollection<SalesOrderLine> SalesOrderLines { get; set; }
        public ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
    }
}