using HrevertCRM.Data.Mapper;
using HrevertCRM.Entities;

namespace HrevertCRM.Data
{
    public class RetailerToRetailerViewModelMapper : MapperBase<Retailer, RetailerViewModel>
    {
        public override Retailer Map(RetailerViewModel viewModel)
        {
            return new Retailer
            {
                Id = viewModel.Id ?? 0,
                DistibutorId = viewModel.DistibutorId ?? 0,
                RetailerId = viewModel.RetailerId ?? 0,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                Version = viewModel.Version
            };
        }

        public override RetailerViewModel Map(Retailer entity)
        {
            return new RetailerViewModel
            {
                Id = entity.Id,
                DistibutorId = entity.DistibutorId,
                RetailerId = entity.RetailerId,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                Version = entity.Version
            };
        }
    }
}
