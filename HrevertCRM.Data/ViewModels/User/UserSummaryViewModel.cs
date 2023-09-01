using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data.ViewModels.User
{
    public class UserSummaryViewModel
    {
        public string Id { get; set; }
        [StringLength(15)]
        public string FirstName { get; set; }
        [StringLength(15)]
        public string MiddleName { get; set; }
        [StringLength(15)]
        public string LastName { get; set; }
        [StringLength(200)]
        public string SecurityGroupNames { get; set; }

        public bool Active { get; set; }

    }
}
