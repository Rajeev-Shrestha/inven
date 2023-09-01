namespace HrevertCRM.Entities
{
    // This class holds members of different security groups.
    public class SecurityGroupMember : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int SecurityGroupId { get; set; } // Security Group ID
        public string MemberId { get; set; } //UserId of the member
        public bool WebActive { get; set; }

        public virtual ApplicationUser MemberUser { get; set; } //Member of the group
        public SecurityGroup SecurityGroup { get; set; } // Security Group
    }
}