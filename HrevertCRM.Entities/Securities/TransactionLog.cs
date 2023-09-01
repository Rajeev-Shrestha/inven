using System;

namespace HrevertCRM.Entities
{
    public class TransactionLog : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int SecurityId { get; set; } //security needed to make modification. identify what was done. Id used here but securitycode is more important
        public int ChangedItemId { get; set; } //Id of entity that was changed
        public bool NotificationProcessed { get; set; }
        public int ItemType { get; set; } //entity type like product, security etc
        public bool WebActive { get; set; }
        public string UserId { get; set; }

        //public ApplicationUser ApplicationUser { get; set; }
        public Security Security { get; set; }  //security code
    }
}
