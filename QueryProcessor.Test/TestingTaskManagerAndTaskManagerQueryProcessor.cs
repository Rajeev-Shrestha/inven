using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;
using System;
using Xunit;

namespace QueryProcessor.Test
{
    public class TestingTaskManagerAndTaskManagerQueryProcessor
    {
        private readonly ITaskManagerQueryProcessor _taskManagerQueryProcessor;
        public TestingTaskManagerAndTaskManagerQueryProcessor(ITaskManagerQueryProcessor taskManagerQueryProcessor)
        {
            _taskManagerQueryProcessor = taskManagerQueryProcessor;
        }
        [Theory]
        [InlineData(1)]
        public void CreateTaskManagerShouldReturnId(int task)
        {
            var taskData = new TaskManager
            {
                TaskTitle = "Horse",
                TaskDescription = "Feed Horse",
                TaskStartDateTime = DateTime.Now,
                TaskEndDateTime = DateTime.Now.AddDays(1),
                Status = Hrevert.Common.TaskReminderStatus.Pending,
                TaskPriority = Hrevert.Common.Priority.Normal,
                CompletePercentage = 12,
                Reminder = true,
                CompanyId = 1,
                ReminderStartDateTime = DateTime.Now,
                ReminderEndDateTime = DateTime.Now.AddHours(3),
                TaskAssignedToUser = "3d55ba45-f47b-4c3c-bca7-2c77d2acc3fd",
                Active = true
            };
            var tasks = _taskManagerQueryProcessor.Insert(taskData);
            Assert.True(tasks.TaskId > 0);
        }
    }
}
