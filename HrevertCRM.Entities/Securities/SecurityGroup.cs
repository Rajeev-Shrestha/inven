using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    // This class defines security group
    public class SecurityGroup : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public bool WebActive { get; set; }


        public virtual ICollection<SecurityGroupMember> Members { get; set; }
        public virtual ICollection<SecurityRight> Rights { get; set; }
    }
}
