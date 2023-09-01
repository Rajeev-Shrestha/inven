namespace HrevertCRM.Entities
{
    public class Retailer : BaseEntity
    {
        public int Id { get; set; }
        public int DistibutorId { get; set; }
        public int RetailerId { get; set; }
    }
}
