using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class EmailSetting : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Sender { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public EncryptionType EncryptionType { get; set; }
        public bool RequireAuthentication { get; set; }
        public bool WebActive { get; set; }
    }
}
