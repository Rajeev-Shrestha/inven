using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class DiscountToDiscountViewModelMapper : MapperBase<Entities.Discount, DiscountViewModel>
    {
        public override Entities.Discount Map(DiscountViewModel viewModel)
        {
            return new Entities.Discount
            {
                Id = viewModel.Id ?? 0,
                ItemId = viewModel.ItemId,
                CategoryId = viewModel.CategoryId,
                DiscountType = viewModel.DiscountType,
                DiscountStartDate = viewModel.DiscountStartDate,
                DiscountEndDate = viewModel.DiscountEndDate,
                MinimumQuantity = viewModel.MinimumQuantity,
                CustomerId = viewModel.CustomerId,
                CustomerLevelId = viewModel.CustomerLevelId,
                DiscountValue = viewModel.DiscountValue ?? 0,

                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId
            };
        }

        public override DiscountViewModel Map(Entities.Discount entity)
        {
            var discountViewModel =  new DiscountViewModel
            {
                Id = entity.Id,
                ItemId = entity.ItemId,
                CategoryId = entity.CategoryId,
                DiscountType = entity.DiscountType,
                DiscountStartDate = entity.DiscountStartDate,
                DiscountEndDate = entity.DiscountEndDate,
                MinimumQuantity = entity.MinimumQuantity,
                CustomerId = entity.CustomerId,
                CustomerLevelId = entity.CustomerLevelId,
                DiscountValue = entity.DiscountValue,

                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version,
                CompanyId = entity.CompanyId
            };
            if (entity.Product != null)
                discountViewModel.ItemName = entity.Product.Name;
            else if (entity.ProductCategory != null)
                discountViewModel.ItemName = entity.ProductCategory.Name;
            else if (entity.Customer != null)
                discountViewModel.ItemName = entity.Customer.DisplayName;
            else if (entity.CustomerLevel != null)
                discountViewModel.ItemName = entity.CustomerLevel.Name;
            else
                discountViewModel.ItemName = "No Name";
            return discountViewModel;
        }
    }
}

