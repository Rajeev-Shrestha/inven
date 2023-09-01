using System;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.ViewModels
{
    public class AccountViewModel : IWebItem
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Account Code is required")]
        [StringLength(40, ErrorMessage = "Account Code can be at most 40 characters.")]
        public string AccountCode { get; set; }

        [StringLength(100, ErrorMessage = "Account Description can be at most 100 characters.")]
        public string AccountDescription { get; set; }

        [EnumDataType(typeof(AccountType))]
        public AccountType AccountType { get; set; }

        public int? ParentAccountId { get; set; } //Nullable for system defined, user must choose one

        public decimal CurrentBalance { get; set; }

        [EnumDataType(typeof(AccountCashFlowType))]
        public AccountCashFlowType AccountCashFlowType { get; set; } //Detailed Level

       [EnumDataType(typeof(AccountDetailType))]
        public AccountDetailType AccountDetailType { get; set; } // Detail Type

        public bool BankAccount { get; set; }

        public bool MainAccount { get; set; } //Cannot be deleted, deactivated

        [EnumDataType(typeof(AccountLevel))]
        public AccountLevel Level { get; set; } //general in tree only, detail is used in transaction

        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }

        
    }
}
