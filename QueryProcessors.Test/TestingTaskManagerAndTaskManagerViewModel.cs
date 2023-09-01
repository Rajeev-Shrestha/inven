using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hrevert.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingTaskManagerAndTaskManagerViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_TaskManager_And_TaskManagerViewModel(int id)
        {
            var vm = new TaskManager()
            {
                TaskId = 1,
                TaskTitle = "Horse",
                TaskDescription = "Feed Horse",
                TaskStartDateTime = DateTime.Now,
                TaskEndDateTime = DateTime.Now.AddHours(2),
                TaskPriority = Priority.High,
                Status = TaskReminderStatus.NotStarted,
                CompanyId = 1,
                CompletePercentage = 12,
                Reminder = false,
                ReminderStartDateTime = DateTime.Now,
                ReminderEndDateTime = DateTime.Now.AddHours(3),
                TaskAssignedToUser = "5b42b49b-ba27-47bb-b29e-39ec442229b1",
                DocId = "1",
                DocType = EntityId.User

            };
            var mappedTaskManagerVm = new HrevertCRM.Data.Mapper.TaskManagerToTaskManagerViewModelMapper().Map(vm);
            var taskManager = new HrevertCRM.Data.Mapper.TaskManagerToTaskManagerViewModelMapper().Map(mappedTaskManagerVm);
            //Test FeaturedItem and mapped FeaturedItem are same
            var res = true;

            PropertyInfo[] mappedproperties = taskManager.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(taskManager) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(taskManager) == null)
                    {
                        res = false;
                        break;

                    }
                    res = propertyValuePair.GetValue(taskManager).Equals(propertyValuePair.GetValue(vm));
                    if (!res)
                    {
                        break;
                    }
                }

            }
            Assert.True(res);

        }
    }
}
