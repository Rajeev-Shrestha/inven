using System.Collections.Generic;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class PaymentTerm : BaseEntity, IWebItem
    {
        //Defines delivery method like Company Delivery, Company Pickup, Courier etc
        //One to one with Sales Order, Purchase Order
        //eg, CREDIT CARD, NET 30, NET 60 2%, NET 1 DAY, C.O.D, PREPAID

        public int Id { get; set; }
        public string TermCode { get; set; }
        public string TermName { get; set; }
        public string Description { get; set; }
        public TermType TermType { get; set; }
        public DueDateType DueDateType { get; set; } 
        public DueType DueType { get; set; } //Prepay, On Account, COD
        public int DueDateValue { get; set; }
        public PaymentDiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; } //Percentage
        public decimal DiscountDays { get; set; } //Percentage
        public bool WebActive { get; set; }


        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Vendor> Vendors { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

    }
}