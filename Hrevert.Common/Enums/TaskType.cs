using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hrevert.Common
{
    public enum TaskReminderStatus 
    {
        NotStarted= 0,
        Started = 1, 
        Pending = 2,
        Committed= 3,
        Done= 4
    }

    public enum Priority
    {
        Normal = 0,
        High = 1,
        Urgent=2
    }
}
