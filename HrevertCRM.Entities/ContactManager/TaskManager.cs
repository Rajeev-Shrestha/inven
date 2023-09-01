using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrevertCRM.Entities.ContactManager
{
    public class TaskManager: BaseEntity
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskStartDateTime { get; set; }
        public DateTime TaskEndDateTime { get; set; }
        public int UserId { get; set; }

        public virtual ApplicationUser applicationUser { get; set; }
    }
}
