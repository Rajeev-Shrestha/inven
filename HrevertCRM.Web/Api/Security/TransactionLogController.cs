using System;
using System.Linq;
using System.Security;
using Hrevert.Common.Constants.Others;
using HrevertCRM.Data;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Data.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Engines;

namespace HrevertCRM.Web.Api
{
    [Route("api/[controller]")]
    public class TransactionLogController : Controller
    {
        private readonly ITransactionLogQueryProcessor _transactionLogQueryProcessor;
        private readonly ISecurityQueryProcessor _securityQueryProcessor;
        private readonly ILogger _logger;

        public TransactionLogController(ITransactionLogQueryProcessor transactionLogQueryProcessor,
            IDbContext context, ILoggerFactory factory, 
            IPagedDataRequestFactory pagedDataRequestFactory,
            ISecurityQueryProcessor securityQueryProcessor)
        {
            _transactionLogQueryProcessor = transactionLogQueryProcessor;
            _securityQueryProcessor = securityQueryProcessor;
            _logger = factory.CreateLogger<TransactionLogController>();
        }
     
        [HttpGet("{id}", Name = "GetTransactionLog")]
        public ObjectResult Get(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewTransactionLogs))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            return Ok(_transactionLogQueryProcessor.GetTransactionLog(id));
        }

        [HttpPost]
        public ObjectResult Create([FromBody] TransactionLogViewModel transactionLogViewModel)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.AddTransactionLog))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);

            if(!ModelState.IsValid) return BadRequest(new
            {
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
            });

            var mapper = new TransactionLogToTransactionLogViewModelMapper();
            var newTransactionLog = mapper.Map(transactionLogViewModel);
            try
            {
                var savedTransactionLog = _transactionLogQueryProcessor.Save(newTransactionLog);
                transactionLogViewModel = mapper.Map(savedTransactionLog);
                return Ok(transactionLogViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int) SecurityId.AddTransactionLog, ex, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return BadRequest(new
                {
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray()
                });
            }
        }

        [HttpDelete("{id}")]
        public ObjectResult Delete(int id)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.DeleteTransactionLog))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            var isDeleted = false;
            try
            {
                isDeleted = _transactionLogQueryProcessor.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.AddTransactionLog, ex, ex.Message);
            }
            return Ok(isDeleted);
        }

        [HttpGet]
        [Route("getTransactionLogs/{active}")]
        public ObjectResult GetTransactionLogs(bool active)
        {
            if (!_securityQueryProcessor.VerifyUserHasRight((long)SecurityId.ViewTransactionLogs))
                return StatusCode(401, OtherConstants.SecurityException.UserDoesNotHaveRightException);
            try
            {
                var mapper = new TransactionLogToTransactionLogViewModelMapper();
                return Ok(_transactionLogQueryProcessor.GetTransactionLogs(active).Select(s => mapper.Map(s)));
            }
            catch (Exception ex)
            {
                _logger.LogCritical((int)SecurityId.ViewTransactionLogs, ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
