
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class FiscalYearViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Fiscal Year name is required")]
        [StringLength(20, ErrorMessage = "Fiscal Year Name can be at most 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fiscal Year start date is required")]
       // [StringLength(11, ErrorMessage = "Start Date can be at most 11 characters.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Fiscal Year end date is required")]
       // [StringLength(11, ErrorMessage = "End Date can be at most 11 characters.")]
        public DateTime EndDate { get; set; }

        public List<FiscalPeriodViewModel> FiscalPeriodViewModels { get; set; }

        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
        public bool WebActive { get; set; }
    }
}
