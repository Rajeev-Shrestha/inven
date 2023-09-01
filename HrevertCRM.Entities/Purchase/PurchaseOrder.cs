using System;
using System.Collections.Generic;
using Hrevert.Common.Enums;


namespace HrevertCRM.Entities
{
    public class PurchaseOrder : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string SalesOrderNumber { get; set; } //Reference sales order number from vendor
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public int FiscalPeriodId { get; set; }
        public int PaymentTermId { get; set; }
        public int DeliveryMethodId { get; set; }
        public PurchaseOrderType OrderType { get; set; }
        public bool FullyPaid { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int BillingAddressId { get; set; }
        public int ShippingAddressId { get; set; }
        public string PurchaseRepId { get; set; }
        public int VendorId { get; set; }
        public DateTime InvoicedDate { get; set; }
        public DateTime PaymentDueOn { get; set; }
        public bool WebActive { get; set; }

        public Address BillingAddress { get; set; }
        public PaymentTerm PaymentTerm { get; set; }
        public Company Company { get; set; }
        public FiscalPeriod FiscalPeriod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ApplicationUser PurchaseRep { get; set; }
        public Vendor Vendor { get; set; }
        public ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
    }
}
