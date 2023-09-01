using System;
using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class FiscalYear : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool WebActive { get; set; }
      
        public Company Company { get; set; }  
         
        public virtual ICollection<FiscalPeriod> FiscalPeriods { get; set; }
    }
}
