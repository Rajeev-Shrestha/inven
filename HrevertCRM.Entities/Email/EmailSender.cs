using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace HrevertCRM.Entities
{
    public class EmailSender : BaseEntity
    {
        public int Id { get; set; }
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsEmailSent { get; set; }
        public string EmailNotSentCause { get; set; }

        public ICollection<IFormFile> Files { get; set; }
    }
}
