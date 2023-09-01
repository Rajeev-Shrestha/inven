using System;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common;
using HrevertCRM.Entities;


namespace HrevertCRM.Data.ViewModels
{
    public class TaskManagerViewModel
    {
        public int? TaskId { get; set; }
        [Required(ErrorMessage = "Task title Required.")]
        [StringLength(40, ErrorMessage = "Title can be at most 40 character.")]
        public string TaskTitle { get; set; }
        [Required(ErrorMessage = "Task Description required.")]
        [StringLength(200, ErrorMessage = "Task Description can be at most 200 character")]
        public string TaskDescription { get; set; }
        [Required(ErrorMessage = "Start date required.")]
        public DateTime TaskStartDateTime { get; set; }
        [Required(ErrorMessage = "End date required.")]
        public DateTime TaskEndDateTime { get; set; }
        public TaskReminderStatus Status { get; set; }
        [Required(ErrorMessage = "Please choose the priority of the task.")]
        public Priority TaskPriority { get; set; }
      // [Range(1,100, ErrorMessage = "Range can be between 1 to 100.")]
        public int CompletePercentage { get; set; }
        public bool Reminder { get; set; }
        [Required(ErrorMessage = "Reminder start date required.")]
        public DateTime ReminderStartDateTime { get; set; }
        [Required(ErrorMessage = "Reminder end date required.")]
        public DateTime ReminderEndDateTime { get; set; }
        [Required(ErrorMessage = "Please assign the task.")]
        public string TaskAssignedToUser { get; set; }
        public string DocId { get; set; }
        public int CompanyId { get; set; }
        public EntityId? DocType { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
        public bool HasTaskAssignRights { get; set; }
    }
}
