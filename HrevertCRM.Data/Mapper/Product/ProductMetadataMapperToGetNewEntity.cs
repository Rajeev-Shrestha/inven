using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class ProductMetadataMapperToGetNewEntity : MapperBase<ProductMetadata, ProductMetadataViewModel>
    {
        public override ProductMetadata Map(ProductMetadataViewModel viewModel)
        {
            return new ProductMetadata
            {
                Id = viewModel.Id ?? 0,
                ProductId = viewModel.ProductId,
                MediaType = viewModel.MediaType,
                MediaUrl = viewModel.MediaUrl,
                WebActive = viewModel.WebActive,
                Active = viewModel.Active,
                Version = viewModel.Version,
                CompanyId = viewModel.CompanyId
            };
        }

        public override ProductMetadataViewModel Map(ProductMetadata entity)
        {
            return new ProductMetadataViewModel
            {
                //Id = entity.Id,
                ProductId = entity.ProductId,
                MediaType = entity.MediaType,
                MediaUrl = entity.MediaUrl,
                WebActive = entity.WebActive,
                Active = entity.Active,
                Version = entity.Version,
                CompanyId = entity.CompanyId
            };
        }
    }
}
