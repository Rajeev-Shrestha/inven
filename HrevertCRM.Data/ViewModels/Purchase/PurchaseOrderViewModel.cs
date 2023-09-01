using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class PurchaseOrderViewModel : IWebItem
    {
        public int? Id { get; set; }

        public string PurchaseOrderCode { get; set; }

        public string SalesOrderNumber { get; set; } //Reference sales order number from vendor

        [Required(ErrorMessage = "Order date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        public DateTime DueDate { get; set; }

        [EnumDataType(typeof(PurchaseOrderStatus))]
        public PurchaseOrderStatus Status { get; set; }

        [EnumDataType(typeof(PurchaseOrderType))]
        public PurchaseOrderType OrderType { get; set; }
        
        public bool FullyPaid { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime InvoicedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PaymentDueOn { get; set; }

        public int FiscalPeriodId { get; set; }
        public int PaymentTermId { get; set; }
        public int DeliveryMethodId { get; set; }
        public int BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public string PurchaseRepId { get; set; }
        public int VendorId { get; set; }

        public byte[] Version { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public bool WebActive { get; set; }

        public ICollection<PurchaseOrderLineViewModel> PurchaseOrderLines { get; set; }
        public string VendorName { get; set; }

    }
}
