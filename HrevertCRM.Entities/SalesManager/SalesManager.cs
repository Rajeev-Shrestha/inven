namespace HrevertCRM.Entities
{
    public class SalesManager : BaseEntity
    {
        //May be we will not be using this class in future
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Level { get; set; }
        public Company Company { get; set; }
        //public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
