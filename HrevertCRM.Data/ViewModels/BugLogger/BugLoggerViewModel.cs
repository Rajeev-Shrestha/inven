using System;

namespace HrevertCRM.Data.ViewModels
{
    public class BugLoggerViewModel
    {
        public int? Id { get; set; }
        public string Message { get; set; }
        public DateTime BugAdded { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public byte[] Version { get; set; }
    }
}
