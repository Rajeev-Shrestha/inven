using System;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class CustomerLoginEventViewModel
    {
        public int? Id { get; set; }

        public int? CustomerId { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime LockedTime { get; set; }

        [EnumDataType(typeof(LockType))]
        public LockType LockType { get; set; }

        [Range(typeof(bool), "true", "false", ErrorMessage = "The field Locked must be checked.")]
        public bool Locked { get; set; }

        public int FailedAttemptCount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LoginTime { get; set; }
    }
}
