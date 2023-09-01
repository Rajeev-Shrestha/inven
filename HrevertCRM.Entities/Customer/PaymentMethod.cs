using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class PaymentMethod : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string MethodCode { get; set; }
        public string MethodName { get; set; }
        public int AccountId { get; set; } //TODO: will be related to COA
        public string ReceipentMemo { get; set; }
        public bool WebActive { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Vendor> Vendors { get; set; }
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }

    }
}
