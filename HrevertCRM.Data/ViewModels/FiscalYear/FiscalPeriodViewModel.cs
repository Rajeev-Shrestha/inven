using System;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class FiscalPeriodViewModel
    {
        public int? Id { get; set; }

        public int FiscalYearId { get; set; }

        [Required(ErrorMessage = "Fiscal Period name is required")]
        [StringLength(20, ErrorMessage = "Fiscal Period Name can be at most 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fiscal Period start date is required")]
        //[ErrorMessage = "Start Date can be at most 11 characters.")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Fiscal Period end date is required")]
      //[StringLength(11, ErrorMessage = "End Date can be at most 11 characters.")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        public byte[] Version { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
    }
}
