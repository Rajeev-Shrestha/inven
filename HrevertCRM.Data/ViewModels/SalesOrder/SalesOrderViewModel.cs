using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class SalesOrderViewModel : IWebItem
    {
        public int? Id { get; set; }
        public string SalesOrderCode { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public DateTime DueDate { get; set; }

        [EnumDataType(typeof(SalesOrderStatus))]
        public SalesOrderStatus Status { get; set; }

        [StringLength(1000, ErrorMessage = "Sales Policy can be at most 1000 characters.")]
        public string SalesPolicy { get; set; } // TODO: Need to apply default from policy settings

        public bool IsWebOrder { get; set; }

        //[Range(typeof(bool), "true", "false", ErrorMessage = "The field Fully Paid must be checked.")]
        public bool FullyPaid { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int PaymentTermId { get; set; }
        public int PaymentMethodId { get; set; }
        public int FiscalPeriodId { get; set; }

        [EnumDataType(typeof(SalesOrderType))]
        public SalesOrderType OrderType { get; set; }

        public int DeliveryMethodId { get; set; }
        public string SalesRepId { get; set; }
        public int CustomerId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime InvoicedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PaymentDueOn { get; set; }

        [EnumDataType(typeof(ShippingStatus))]
        public ShippingStatus? ShippingStatus { get; set; }
        public decimal? ShippingCost { get; set; }

        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
        public string CustomerName { get; set; }

        public ICollection<SalesOrderLineViewModel> SalesOrderLines { get; set; }
    }
}
