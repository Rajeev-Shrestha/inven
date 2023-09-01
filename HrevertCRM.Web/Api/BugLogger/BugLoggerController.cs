using System;
using System.Linq;
using HrevertCRM.Data;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    [Route("api/[Controller]")]
    public class BugLoggerController : Controller
    {
        private readonly IBugLoggerQueryProcessor _bugLoggerQueryProcessor;
        private readonly IEmailSenderQueryProcessor _emailSenderQueryProcessor;
        private readonly IDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BugLoggerController> _logger;

        public BugLoggerController(IBugLoggerQueryProcessor bugLoggerQueryProcessor, 
            IEmailSenderQueryProcessor emailSenderQueryProcessor,
            IEmailSettingQueryProcessor emailSettingQueryProcessor,
            ILoggerFactory factory, IDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _bugLoggerQueryProcessor = bugLoggerQueryProcessor;
            _emailSenderQueryProcessor = emailSenderQueryProcessor;
            _userManager = userManager;
            _dbContext = dbContext;

            _logger = factory.CreateLogger<BugLoggerController>();
        }

        [HttpGet]
        [Route("getBugById/{id}")]
        public ObjectResult Get(int id)
        {
            var bug = _bugLoggerQueryProcessor.Get(id);
            var mapper = new BugLoggerToBugLoggerViewModelMapper();
            return Ok(mapper.Map(bug));
        }

        [HttpGet]
        [Route("getAllBugs")]
        public ObjectResult GetAllBugs()
        {
            var bugs = _bugLoggerQueryProcessor.GetAllBugs();
            var mapper = new BugLoggerToBugLoggerViewModelMapper();
            return Ok(mapper.Map(bugs));
        }

        [HttpPost]
        [Route("reportBug")]
        public ObjectResult Create([FromBody] BugLoggerViewModel bugLoggerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            bugLoggerViewModel.Message = bugLoggerViewModel.Message.Trim();
            var mapper = new BugLoggerToBugLoggerViewModelMapper();
            var newBug = mapper.Map(bugLoggerViewModel);
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string message;
                    if (bugLoggerViewModel.Message.EndsWith(","))
                    {
                        var messages =
                            bugLoggerViewModel.Message.Remove(bugLoggerViewModel.Message.Length - 1).Split(',');
                        message = messages[0] + ", is not working. The error message is: " + messages[1];
                    }
                    else
                    {
                        message = bugLoggerViewModel.Message;
                    }
                    newBug.Message = message;
                    newBug.BugAdded = DateTime.Now;
                    var currentUserCompanyId = _userManager.FindByEmailAsync(User.Identity.Name).Result.CompanyId;
                    newBug.CompanyId = currentUserCompanyId;
                    var savedBug = _bugLoggerQueryProcessor.Save(newBug);
                    const string mailTo = "thakurkamlesh10@gmail.com;hackingmindsam@hotmail.com";
                    const string subject = "Bug Found!!!";
                    _emailSenderQueryProcessor.ReportBug(new EmailSenderViewModel
                    {
                        MailTo = mailTo,
                        Subject = subject,
                        Message = message
                    });
                    trans.Commit();
                    return Ok(mapper.Map(savedBug));
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogCritical((int)SecurityId.AddBugLogger, ex, ex.Message);
                    ModelState.AddModelError("", ex.Message);
                    return BadRequest(new
                    {
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(x => x.ErrorMessage)
                    });
                }
            }
        }

        [HttpPut]
        [Route("updateBug")]
        public ObjectResult Put([FromBody] BugLoggerViewModel bugLoggerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            try
            {
                bugLoggerViewModel.Message = bugLoggerViewModel.Message.Trim();
                var mapper = new BugLoggerToBugLoggerViewModelMapper();
                var newBug = mapper.Map(bugLoggerViewModel);

                var updatedBug = _bugLoggerQueryProcessor.Update(newBug);
                return Ok(mapper.Map(updatedBug));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.UpdateCustomer, ex, ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(x => x.ErrorMessage)
                });
            }
        }

    }
}
