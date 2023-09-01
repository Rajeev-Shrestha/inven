using System;
using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class SalesOrder : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string SalesOrderCode { get; set; }
        public string PurchaseOrderNumber { get; set; } // yo utai bata aaucha
        public DateTime DueDate { get; set; } // Order due date
        public SalesOrderStatus Status { get; set; }
        public string SalesPolicy { get; set; } // TODO: Need to apply default from policy settings
        public bool IsWebOrder { get; set; }
        public bool FullyPaid { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int BillingAddressId { get; set; } //sc
        public int ShippingAddressId { get; set; } //sc
        public int PaymentTermId { get; set; } ////sc
        public int PaymentMethodId { get; set; }
        public int FiscalPeriodId { get; set; } 
        public SalesOrderType OrderType { get; set; }
        public int DeliveryMethodId { get; set; }  //sc
        public string SalesRepId { get; set; } //TODO: Done>>CWS -> Company Web Setting 
        public int CustomerId { get; set; } 
        public bool WebActive { get; set; } //true
        public DateTime InvoicedDate { get; set; } 
        public DateTime PaymentDueOn { get; set; }
        public decimal? ShippingCost { get; set; }


        public Address BillingAddress { get; set; }
        //public Address ShippingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentTerm PaymentTerm { get; set; }
        public Company Company { get; set; }
        public FiscalPeriod FiscalPeriod { get; set; }
        public Customer Customer { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ApplicationUser SalesRep { get; set; } //Its a ApplicationUser entity with ICollection of  sales order in ApplcationUser

        public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; }
    }
}
