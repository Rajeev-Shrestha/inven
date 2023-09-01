using System.Linq;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data;
using Hrevert.Common.Constants.Others;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class TaskManagerController: Controller
    {
        private readonly ITaskManagerQueryProcessor _taskManagerQueryProcessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly ILogger<TaskManagerController> _logger;


        public TaskManagerController(
                ITaskManagerQueryProcessor taskManagerQueryProcessor, 
                UserManager<ApplicationUser> userManager,
                 ISecurityQueryProcessor securityQueryProcessor,
                IPagedDataRequestFactory pagedDataRequestFactory,
                ILoggerFactory factory

            )
        {
            _taskManagerQueryProcessor = taskManagerQueryProcessor;
            _userManager = userManager;
            _securityQueryProcessor = securityQueryProcessor;
            _pagedDataRequestFactory = pagedDataRequestFactory;
            _logger = factory.CreateLogger<TaskManagerController>();

        }
        [HttpGet]
        [Route("getAllTasks")]
        public ObjectResult GetAllTasks()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewTasks))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var requestInfo = _pagedDataRequestFactory.Create(Request.GetUri());
                var result = _taskManagerQueryProcessor.GetAll();
                var mapper = new TaskManagerToTaskManagerViewModelMapper();
                var resultData = mapper.Map(result.ToList());
                return Ok(resultData);
            }
            catch (System.Exception ex)
            {

                _logger.LogCritical((int)SecurityId.ViewTasks, ex, ex.Message);
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet]
        [Route("getAllActiveTasks")]
        public ObjectResult GetAllActiveTasks()
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewTasks))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var result = _taskManagerQueryProcessor.GetAllActive();
            var mapper = new TaskManagerToTaskManagerViewModelMapper();
            var resultData = mapper.Map(result.ToList());
            return Ok(resultData);
        }
        [HttpPost]
        [Route("Create")]
        public ObjectResult CreateNewTask([FromBody] TaskManagerViewModel taskManagerViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AssignTask))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
            taskManagerViewModel.CompanyId = currentUserCompanyId;
            taskManagerViewModel.TaskTitle = taskManagerViewModel.TaskTitle.Trim();
            taskManagerViewModel.TaskDescription = taskManagerViewModel.TaskDescription.Trim();
            var mapper = new TaskManagerToTaskManagerViewModelMapper();
            var mappedData = mapper.Map(taskManagerViewModel);
            var result = _taskManagerQueryProcessor.Insert(mappedData);
            taskManagerViewModel = mapper.Map(result);
            return Ok(taskManagerViewModel);
        }
        [HttpDelete("{id}")]
        public void DeleteTask(int id)
        {
            _taskManagerQueryProcessor.Delete(id);
        }
        [HttpGet]
        [Route("activeTasks/{id}")]
        public ObjectResult ActiveTasks(int id)
        {
            return Ok(_taskManagerQueryProcessor.ActiveTask(id));
        }
        [HttpGet]
        [Route("searchTasks")]
        public ObjectResult SearchTasks()
        {
            var requestInfo = _pagedDataRequestFactory.CreateWithSearchStringCompanyActive(Request.GetUri());
            var result = _taskManagerQueryProcessor.SearchTasks(requestInfo);
            return Ok(result);
        }
        [HttpGet]
        [Route("taskAssignRights")]
        public ObjectResult HasTaskAssignRights()
        {
            return Ok(_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AssignTask));
        }
        [HttpPut]
        [Route("Update")]
        public ObjectResult UpdateTask([FromBody] TaskManagerViewModel taskManagerViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.UpdateTask))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if (!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });
            taskManagerViewModel.TaskTitle = taskManagerViewModel.TaskTitle.Trim();
            taskManagerViewModel.TaskDescription = taskManagerViewModel.TaskDescription.Trim();
            var mapper = new TaskManagerToTaskManagerViewModelMapper();
            var mappedData = mapper.Map(taskManagerViewModel);
            var taskById = _taskManagerQueryProcessor.GetTaskManagerByTaskId(mappedData.TaskId);
            if (taskById != null)
            {
                var result = _taskManagerQueryProcessor.Update(mappedData);
                var taskResultViewModel = mapper.Map(result);
                return Ok(taskResultViewModel);
            }
            else
            {
                return Ok(BadRequest("Cannot update task"));
            }
            
        }
        [HttpGet]
        [Route("sortTask/{id}")]
        public ObjectResult SortTask(int id)
        {
            var result = _taskManagerQueryProcessor.SortTask(id);
            return Ok(result);
        }
        [HttpGet]
        [Route("getActiveTaskById/{id}")]
        public ObjectResult GetActiveTaskById(int id)
        {
            var result = _taskManagerQueryProcessor.GetTaskManagerByTaskId(id);
            return Ok(result);
        }

    }
}
