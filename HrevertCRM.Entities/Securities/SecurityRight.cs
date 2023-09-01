namespace HrevertCRM.Entities
{
    public class SecurityRight : BaseEntity, IWebItem
    {
        public int Id { get; set; }    
        public bool Allowed { get; set; }
        public string UserId { get; set; } //UserId of the member
        public int? SecurityGroupId { get; set; } // Security Group ID
        public int SecurityId { get; set; }
        public bool WebActive { get; set; }

        public Security Security { get; set; } // Security Assigned
        public virtual SecurityGroup SecurityGroup { get; set; } // Security Group with this security
        public virtual ApplicationUser MemberUser { get; set; } //User with this security
    }
}
