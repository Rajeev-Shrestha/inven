using System;
using Hrevert.Common.Enums;
using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class DiscountToDiscountViewModelForSlideMapper : MapperBase<Entities.Discount,DiscountViewModelForSlide>
    {
        public override Entities.Discount Map(DiscountViewModelForSlide viewModel)
        {
            throw new NotImplementedException();
        }

        public override DiscountViewModelForSlide Map(Entities.Discount entity)
        {
            var discountVmForslide = new DiscountViewModelForSlide
            {
                Expires = entity.DiscountEndDate != DateTime.MinValue,
                ExpiryDate = entity.DiscountEndDate
            };
            if (entity.Product == null) return discountVmForslide;
            discountVmForslide.OriginalPrice = entity.Product.UnitPrice;
            switch (entity.DiscountType)
            {
                case DiscountType.Fixed:
                    discountVmForslide.SalePrice = entity.Product.UnitPrice - entity.DiscountValue;
                    var discountValue = entity.DiscountValue / entity.Product.UnitPrice * 100;
                    discountVmForslide.DiscountPercentage = Convert.ToDouble(string.Format("{0:0.00}", discountValue));
                    break;
                case DiscountType.Percent:
                    discountVmForslide.SalePrice = entity.Product.UnitPrice - (entity.Product.UnitPrice * entity.DiscountValue / 100);
                    discountVmForslide.DiscountPercentage = entity.DiscountValue;
                    break;
                case DiscountType.None:
                    discountVmForslide.SalePrice = entity.Product.UnitPrice;
                    discountVmForslide.DiscountPercentage = 0;
                    break;
                default:
                    discountVmForslide.SalePrice = entity.Product.UnitPrice;
                    discountVmForslide.DiscountPercentage = 0;
                    break;
            }
            return discountVmForslide;
        }
    }
}
