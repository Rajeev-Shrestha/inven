using System.Collections.Generic;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class AccountNode
    {
        public List<AccountNode> Children = new List<AccountNode>();
        public AccountNode Parent { get; set; }
        public Account Source { get; set; }
    }
}