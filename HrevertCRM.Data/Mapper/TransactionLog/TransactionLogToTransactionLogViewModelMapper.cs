using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class TransactionLogToTransactionLogViewModelMapper : MapperBase<TransactionLog, TransactionLogViewModel>
    {
        public override TransactionLog Map(TransactionLogViewModel transactionLogViewModel)
        {
            return new TransactionLog
            {
                Id = transactionLogViewModel.Id ?? 0,
                SecurityId = transactionLogViewModel.SecurityId,
                Description = transactionLogViewModel.Description,
                ChangedItemId = transactionLogViewModel.ChangedItemId,
                UserId = transactionLogViewModel.UserId,
                ItemType = transactionLogViewModel.ItemType,
                NotificationProcessed = transactionLogViewModel.NotificationProcessed,
                TransactionDate = transactionLogViewModel.TransactionDate,
                Version = transactionLogViewModel.Version,
                WebActive = transactionLogViewModel.WebActive
            };
        }

        public override TransactionLogViewModel Map(TransactionLog entity)
        {
            return new TransactionLogViewModel
            {
                Id = entity.Id,
                SecurityId = entity.SecurityId,
                Description = entity.Description,
                ChangedItemId = entity.ChangedItemId,
                UserId = entity.UserId,
                ItemType = entity.ItemType,
                NotificationProcessed = entity.NotificationProcessed,
                TransactionDate = entity.TransactionDate,
                Version = entity.Version,
                WebActive = entity.WebActive
            };
        }
    }
}
