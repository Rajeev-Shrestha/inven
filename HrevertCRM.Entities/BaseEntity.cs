using System;

namespace HrevertCRM.Entities
{
    public abstract class BaseEntity
    {
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public ApplicationUser LastUser { get; set; }
        public ApplicationUser CreatedByUser { get; set; }

        //Future Use
        //public bool IsDBoardUpdated { get; set; }
        //public DateTime DBoardUpdateTime { get; set; }
    }
}