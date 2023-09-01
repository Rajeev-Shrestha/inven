using System.ComponentModel.DataAnnotations;

namespace HrevertCRM.Data
{
    public class SecurityIdWithDefaultGroupPermisson
    {
        public bool IsAdmin { get; set; }
        public bool IsPurchase { get; set; }
        public bool IsSales { get; set; }
        public bool IsDistributor { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
    }
}