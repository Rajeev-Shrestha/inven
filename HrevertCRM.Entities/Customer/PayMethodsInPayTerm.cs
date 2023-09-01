using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class PayMethodsInPayTerm : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public int PayTermId { get; set; }
        public int PayMethodId { get; set; }
        public bool WebActive { get; set; }
    }
}
