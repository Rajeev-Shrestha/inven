using Hrevert.Common.Enums;

namespace HrevertCRM.Entities
{
    public class Account : BaseEntity, IWebItem
    {
        public int Id { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public AccountType AccountType { get; set; } //Main Account Type
        public AccountCashFlowType AccountCashFlowType { get; set; } //Detailed Level
        public AccountDetailType AccountDetailType { get; set; } // Detail Type
        public Account ParentAccount { get; set; }
        public int?  ParentAccountId { get; set; } //Nullable for system defined, user must choose one
        public decimal CurrentBalance { get; set; }
        public bool BankAccount { get; set; }
        public bool MainAccount { get; set; } //Cannot be deleted, deactivated
        public AccountLevel Level { get; set; } //general in tree only, detail is used in transaction
        public bool WebActive { get; set; }

       
    }
}
