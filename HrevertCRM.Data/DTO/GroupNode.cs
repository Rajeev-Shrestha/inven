using System.Collections.Generic;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.DTO
{
    public class GroupNode
    {
        public List<GroupNode> Children = new List<GroupNode>();
        public GroupNode Parent { get; set; }
        public CustomerContactGroup Source { get; set; }
    }
}
