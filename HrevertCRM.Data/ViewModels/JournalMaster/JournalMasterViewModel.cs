using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hrevert.Common.Enums;

namespace HrevertCRM.Data.ViewModels
{
    public class JournalMasterViewModel
    {
        public int? Id { get; set; }

        [EnumDataType(typeof(JournalType))]
        public JournalType JournalType { get; set; } // General, Purchase, Payment, Sales, Receipt
        
        public bool? Closed { get; set; }

        [StringLength(100, ErrorMessage = "Description can be at most 100 characters.")]
        public string Description { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public int FiscalPeriodId { get; set; }

        public bool? Posted { get; set; }

        [StringLength(200, ErrorMessage = "Description can be at most 200 characters.")]
        public string Note { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PostedDate { get; set; }

        public bool? Printed { get; set; }

        public bool? Cancelled { get; set; }

        [DisplayName("Is System")]
      //  [Range(typeof(bool), "true", "false", ErrorMessage = "The field Is System must be checked.")]
        public bool IsSystem { get; set; } // Indicated journal can't be modified

        public bool WebActive { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
    }
}
