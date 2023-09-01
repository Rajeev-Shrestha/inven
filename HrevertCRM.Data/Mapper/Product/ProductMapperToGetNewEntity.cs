using HrevertCRM.Data.ViewModels;

namespace HrevertCRM.Data.Mapper
{
    public class ProductMapperToGetNewEntity : MapperBase<Entities.Product, ProductViewModel>
    {
        public override Entities.Product Map(ProductViewModel viewModel)
        {
            return new Entities.Product
            {
                Id = viewModel.Id ?? 0,
                Code = viewModel.Code,
                Name = viewModel.Name,
                QuantityOnHand = viewModel.QuantityOnHand,
                QuantityOnOrder = viewModel.QuantityOnOrder,
                UnitPrice = viewModel.UnitPrice,
                ShortDescription = viewModel.ShortDescription,
                LongDescription = viewModel.LongDescription,
                Version = viewModel.Version,
                WebActive = viewModel.WebActive,
                Commissionable = viewModel.Commissionable,
                CommissionRate = viewModel.CommissionRate,
                Active = viewModel.Active,
                ProductType = viewModel.ProductType,
                CompanyId = viewModel.CompanyId,
                AllowBackOrder = viewModel.AllowBackOrder
            };
        }

        public override ProductViewModel Map(Entities.Product entity)
        {
            return new ProductViewModel
            {
                //Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                QuantityOnHand = entity.QuantityOnHand,
                QuantityOnOrder = entity.QuantityOnOrder,
                UnitPrice = entity.UnitPrice,
                ShortDescription = entity.ShortDescription,
                LongDescription = entity.LongDescription,
                Version = entity.Version,
                WebActive = entity.WebActive,
                Commissionable = entity.Commissionable,
                CommissionRate = entity.CommissionRate,
                Active = entity.Active,
                ProductType = entity.ProductType,
                CompanyId = entity.CompanyId,
                AllowBackOrder = entity.AllowBackOrder
            };
        }
    }
}
