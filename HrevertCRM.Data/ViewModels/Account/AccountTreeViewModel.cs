using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public sealed class AccountTreeViewModel
    {
        public AccountTreeViewModel()
        {
            Children = new List<AccountTreeViewModel>();
        }

        public int? Id { get; set; }
        [StringLength(40)]
        public string AccountCode { get; set; }
        [StringLength(100)]
        public string AccountDescription { get; set; }
        public AccountType AccountType { get; set; }
        public int? ParentAccountId { get; set; } //Nullable for system defined, user must choose one
        public decimal CurrentBalance { get; set; }
        public bool BankAccount { get; set; }
        public bool MainAccount { get; set; } //Cannot be deleted, deactivated
        public AccountLevel Level { get; set; } //general in tree only, detail is used in transaction
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public List<AccountTreeViewModel> Children { get; set; }
        public bool WebActive { get; set; }
        public byte[] Version { get; set; }
        public AccountCashFlowType AccountCashFlowType { get; set; }
        public AccountDetailType AccountDetailType { get; internal set; }
    }
}