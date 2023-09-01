using System;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using HrevertCRM.Data.Mapper;
using PagedTaskDataInquiryResponse = HrevertCRM.Data.Common.PagedDataInquiryResponse<HrevertCRM.Data.ViewModels.TransactionLogViewModel>;

namespace HrevertCRM.Data.QueryProcessors
{
    public class TransactionLogQueryProcessor : QueryBase<TransactionLog>, ITransactionLogQueryProcessor
    {
        public TransactionLogQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public TransactionLog Save(TransactionLog transactionLog)
        {
            transactionLog.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<TransactionLog>().Add(transactionLog);
            _dbContext.SaveChanges();
            return transactionLog;
        }

        public bool Exists(Expression<Func<TransactionLog, bool>> @where)
        {
           return _dbContext.Set<TransactionLog>().Any(@where);
        }

        public TransactionLog GetTransactionLog(int transactionLogId)
        {
            var transactionLog = _dbContext.Set<TransactionLog>().FirstOrDefault(x => x.Id == transactionLogId);
            return transactionLog;
        }

        public PagedTaskDataInquiryResponse GetTransactionLogs(PagedDataRequest requestInfo, Expression<Func<TransactionLog, bool>> @where = null)
        {
            var query = _dbContext.Set<TransactionLog>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as ProductCategory[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var mapper = new TransactionLogToTransactionLogViewModelMapper();
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).Select(x => mapper.Map(x)).ToList();
            var queryResult = new QueryResult<TransactionLogViewModel>(docs, totalItemCount, requestInfo.PageSize);

            var inquiryResponse = new PagedTaskDataInquiryResponse()
            {
                Items = docs,
                TotalRecords = totalItemCount,
                PageCount = queryResult.TotalPageCount,
                PageNumber = requestInfo.PageNumber,
                PageSize = requestInfo.PageSize
            };

            return inquiryResponse;
        }

        public TransactionLog[] GetTransactionLog(Expression<Func<TransactionLog, bool>> @where = null)
        {
            var query = _dbContext.Set<TransactionLog>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public bool Delete(int transactionLogId)
        {
            var doc = GetTransactionLog(transactionLogId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<TransactionLog>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public IQueryable<TransactionLog> GetTransactionLogs(bool active)
        {
            var transactionLogs = _dbContext.Set<TransactionLog>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                transactionLogs = transactionLogs.Where(x => x.Active);
            return transactionLogs;
        }
    }
}
