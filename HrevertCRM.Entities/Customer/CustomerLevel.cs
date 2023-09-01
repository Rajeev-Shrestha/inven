using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class CustomerLevel : BaseEntity
    {
        //like retail, wholesale, distributor, none, partners
        public int Id { get; set; }
        public string Name { get; set; } //length 50

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }

    }
}