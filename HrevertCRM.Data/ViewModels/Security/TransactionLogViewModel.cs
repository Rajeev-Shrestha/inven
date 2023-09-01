using System;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class TransactionLogViewModel
    {
        public int? Id { get; set; }
        public DateTime TransactionDate { get; set; }

        [StringLength(100, ErrorMessage = "Description can be at most 100 characters.")]
        public string Description { get; set; }

        public int SecurityId { get; set; } //security needed to make modification. identify what was done. Id used here but securitycode is more important
        public int ChangedItemId { get; set; } //Id of entity that was changed
        public bool NotificationProcessed { get; set; }
        public int ItemType { get; set; } //entity  type like product, security etc
        public string UserId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
    }
}
