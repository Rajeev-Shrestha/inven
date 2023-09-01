using System;
using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class CustomerLoginEvent
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LockedTime { get; set; }
        public int FailedAttemptCount { get; set; } = 0;
        public bool Locked { get; set; }
        public LockType LockType { get; set; }
        public int LockDuration { get; set; } //For how many minutes the user is locked for? like 60, 90 etc.
    }
}

/*
    Will be used to record customer login events in the website. 

    Table: CustmerLoginEvent
    does not need EntityBase as this event is tracked when nobody is logged in
    CustomerId?
    LoginTime
    FailedAttemptCount
    Locked
    LockType- Enum, multiple invalid Attempts, Payment forgery 
 */
