using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels
{
    public class GradeViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        public string GradeName { get; set; }

        [Required(ErrorMessage = "Rank is required")]
        public int Rank { get; set; }
        public int CompanyId { get; set; }
        public byte[] Version { get; set; }
        public bool Active { get; set; }
    }
}
