using System;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ITransactionLogQueryProcessor
    {
        TransactionLog Save(TransactionLog transactionLog);
        bool Exists(Expression<Func<TransactionLog, bool>> @where);
        TransactionLog GetTransactionLog(int transactionLogId);
        PagedDataInquiryResponse<TransactionLogViewModel> GetTransactionLogs(PagedDataRequest requestInfo,
            Expression<Func<TransactionLog, bool>> @where = null);
        TransactionLog[] GetTransactionLog(Expression<Func<TransactionLog, bool>> @where = null);
        bool Delete(int transactionLogId);
        IQueryable<TransactionLog> GetTransactionLogs(bool active);
    }
}
