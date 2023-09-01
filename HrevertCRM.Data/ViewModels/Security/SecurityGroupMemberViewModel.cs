using System.Collections.Generic;

namespace HrevertCRM.Data.ViewModels
{
    public class SecurityGroupMemberViewModel
    {
        public int? Id { get; set; }
        public string MemberId { get; set; }
        public int SecurityGroupId { get; set; }

        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public List<string> MembersOfGroupList { get; set; }
    }
}
