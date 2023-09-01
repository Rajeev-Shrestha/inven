using System;

namespace HrevertCRM.Data.ViewModels
{
    public class DiscountViewModelForSlide
    {
        public bool Expires { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double SalePrice { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountPercentage { get; set; }
    }
}
