using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class EmailSenderViewModel
    {
        public int? Id { get; set; }

        [StringLength(50, ErrorMessage = "Mail From can be at most 50 characters.")]
        public string MailFrom { get; set; }

        [Required(ErrorMessage = "Mail To is required")]
        [StringLength(500, ErrorMessage = "Mail To can be at most 50 characters.")]
        public string MailTo { get; set; }

        [StringLength(500, ErrorMessage = "Carbon Copy can be at most 500 characters.")]
        public string Cc { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject can be at most 200 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000, ErrorMessage = "Message can be at most 2000 characters.")]
        public string Message { get; set; }

        public bool IsEmailSent { get; set; }

        public string EmailNotSentCause { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
    }
}
