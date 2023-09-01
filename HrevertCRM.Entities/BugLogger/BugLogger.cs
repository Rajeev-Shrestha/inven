using System;

namespace HrevertCRM.Entities
{
    public class BugLogger : BaseEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime BugAdded { get; set; } = DateTime.Now;

    }
}
