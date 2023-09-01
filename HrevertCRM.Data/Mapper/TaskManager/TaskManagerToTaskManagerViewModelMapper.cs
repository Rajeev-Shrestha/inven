using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class TaskManagerToTaskManagerViewModelMapper : MapperBase<TaskManager,TaskManagerViewModel>
    {
        public override TaskManager Map(TaskManagerViewModel viewModel)
        {
           return  new TaskManager()
           {
               TaskId = viewModel.TaskId ?? 0,
               TaskTitle = viewModel.TaskTitle,
               TaskDescription = viewModel.TaskDescription,
               TaskStartDateTime = viewModel.TaskStartDateTime,
               TaskEndDateTime = viewModel.TaskEndDateTime,
               TaskPriority = viewModel.TaskPriority,
               Status = viewModel.Status,
               DocId = viewModel.DocId,
               DocType = viewModel.DocType??0,
               Active = viewModel.Active,
               CompletePercentage = viewModel.CompletePercentage,
               Reminder =  viewModel.Reminder,
               ReminderStartDateTime = viewModel.ReminderStartDateTime,
               ReminderEndDateTime = viewModel.ReminderEndDateTime,
               TaskAssignedToUser = viewModel.TaskAssignedToUser,
               CompanyId = viewModel.CompanyId,
               Version= viewModel.Version
           };
        }

        public override TaskManagerViewModel Map(TaskManager entity)
        {
            return new TaskManagerViewModel()
            {
                TaskId = entity.TaskId,
                TaskTitle = entity.TaskTitle,
                TaskDescription = entity.TaskDescription,
                TaskStartDateTime = entity.TaskStartDateTime,
                TaskEndDateTime = entity.TaskEndDateTime,
                TaskPriority = entity.TaskPriority,
                Status = entity.Status,
                DocId = entity.DocId,
                DocType = entity.DocType,
                Active = entity.Active,
                CompletePercentage = entity.CompletePercentage,
                Reminder = entity.Reminder,
                ReminderStartDateTime = entity.ReminderStartDateTime,
                ReminderEndDateTime = entity.ReminderEndDateTime,
                TaskAssignedToUser = entity.TaskAssignedToUser,
                CompanyId = entity.CompanyId,
                Version= entity.Version
            };
        }
    }
}
