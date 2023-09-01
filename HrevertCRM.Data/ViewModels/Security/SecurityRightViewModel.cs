namespace HrevertCRM.Data.ViewModels
{
    public class SecurityRightViewModel
    {
        public int? Id { get; set; }
        public bool Allowed { get; set; }
        public string UserId { get; set; } //UserId of the member
        public int SecurityId { get; set; }
        public int? SecurityGroupId { get; set; } // Security Group ID

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }

        public string SecurityName { get; set; }
    }
}
