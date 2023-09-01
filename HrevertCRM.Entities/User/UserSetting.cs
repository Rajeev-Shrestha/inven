namespace HrevertCRM.Entities
{
    public class UserSetting : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public EntityId EntityId { get; set; }
        public int PageSize { get; set; }
    }
}
