using System;
using Hrevert.Common;

namespace HrevertCRM.Entities
{
    public class TaskManager: BaseEntity
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskStartDateTime { get; set; }
        public DateTime TaskEndDateTime { get; set; }
        public TaskReminderStatus Status { get; set; }
        public Priority TaskPriority { get; set; }
        public int CompletePercentage { get; set; }
        public bool Reminder { get; set; }
        public DateTime ReminderStartDateTime { get; set; }
        public DateTime ReminderEndDateTime { get; set; }
        public string TaskAssignedToUser { get; set; }
        public string DocId { get; set; }
        public EntityId DocType { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
