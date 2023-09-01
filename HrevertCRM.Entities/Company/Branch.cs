using System;

namespace HrevertCRM.Entities
{
    public class Branch : BaseEntity, IModificationHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Branch NewBranch { get; set; }
        public Company Company { get; set; }
    }
}
