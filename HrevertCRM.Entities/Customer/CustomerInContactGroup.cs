namespace HrevertCRM.Entities
{
    public class CustomerInContactGroup : BaseEntity
    {
        public int Id { get; set; }
        public int ContactGroupId { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual CustomerContactGroup ContactGroup { get; set; }
    }
}