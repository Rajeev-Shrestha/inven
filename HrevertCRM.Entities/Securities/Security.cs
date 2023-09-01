using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class Security : BaseEntity, IWebItem
    {
        public int Id { get; set; }  
        public int SecurityCode { get; set; }
        public string SecurityDescription { get; set; }
        public bool WebActive { get; set; }


        public virtual ICollection<SecurityRight> SecurityRights { get; set; } //assigned rights to user and group
        public virtual ICollection<TransactionLog> TransactionLogs { get; set; }
    }
}
