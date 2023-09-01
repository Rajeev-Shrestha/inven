namespace HrevertCRM.Entities
{
    public class Distributor : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        public Company Company { get; set; }
    }
}
