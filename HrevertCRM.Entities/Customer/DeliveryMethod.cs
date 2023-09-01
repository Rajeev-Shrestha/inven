using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class DeliveryMethod : BaseEntity, IWebItem
    {
        //Defines delivery method like Company Delivery, Company Pickup, Courier etc
        //One to one with Sales Order, Purchase Order
        public int Id { get; set; }
        public string DeliveryCode { get; set; }
        public string Description { get; set; }
        public bool WebActive { get; set; }

        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<DeliveryRate> DeliveryRates { get; set; }

    }
}