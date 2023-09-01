using HrevertCRM.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Hrevert.Common;
using HrevertCRM.Entities;
using Xunit;
using System.Linq;

namespace ApiControllers.Test
{
    public class TestingTaskManagerAndTaskManagerController
    {
        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllTasks(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/taskManager/getAllActiveTasks");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            Assert.True(content.ToList().Count>0);
        }
        [Theory]
        [InlineData(1)]
        // [Fact]
        public async void CreateTaskShouldReturnId(int test)
        {
            var taskData = new TaskManagerViewModel
            {
                TaskTitle = "Horse",
                TaskDescription = "Feed Horse",
                TaskStartDateTime = DateTime.Now,
                TaskEndDateTime = DateTime.Now.AddDays(1),
                Status = TaskReminderStatus.NotStarted,
                TaskPriority = Priority.Normal,
                CompletePercentage = 12,
                Reminder = true,
                CompanyId = 1,
                ReminderStartDateTime = DateTime.Now,
                ReminderEndDateTime=DateTime.Now.AddHours(3),
                TaskAssignedToUser = "119b9e19-a1ca-4103-a4be-c9261b0d87b5",
                Active = true
            };
            var json = JsonConvert.SerializeObject(taskData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/taskManager/create", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedSecurity = JsonConvert.DeserializeObject<TaskManagerViewModel>(content);
            Assert.True(returnedSecurity.TaskId > 0);
        }
        [Theory]
        [InlineData(1)]
        public async void updateTaskShouldReturnId(int task)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/taskManager/getActiveTaskById/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var TaskData = JsonConvert.DeserializeObject<TaskManagerViewModel>(content);
            TaskData.TaskTitle = "haha";
            TaskData.TaskDescription = "hahahahahha";
            TaskData.CompletePercentage = 40;
            var json = JsonConvert.SerializeObject(TaskData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/taskManager/update", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var result = responses.Content.ReadAsStringAsync().Result;
            var returnedTask = JsonConvert.DeserializeObject<TaskManagerViewModel>(result);
            Assert.True(returnedTask.TaskId > 0);
        }
        [Theory]
        [InlineData(1)]
        public async void DeleteTaskShouldReturnOk(int task)
        {
            int TaskId = 1;
           
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/taskManager/"+TaskId);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");
        }
    }
   
}
