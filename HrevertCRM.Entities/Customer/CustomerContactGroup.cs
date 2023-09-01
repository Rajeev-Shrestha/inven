using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class CustomerContactGroup : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public bool WebActive { get; set; }

        public List<CustomerInContactGroup> CustomerAndContactGroupList { get; set; }
        public List<int> CustomerIdsInContactGroup { get; set; }
    }
}

