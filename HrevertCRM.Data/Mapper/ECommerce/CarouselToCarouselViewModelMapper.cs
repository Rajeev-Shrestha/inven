using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public class CarouselToCarouselViewModelMapper : MapperBase<Carousel, CarouselViewModel>
    {
        public override Carousel Map(CarouselViewModel viewModel)
        {
            var carousel = new Carousel
            {
                Id = viewModel.Id ?? 0,
                CompanyId = viewModel.CompanyId,
                Active = viewModel.Active,
                WebActive = viewModel.WebActive,
                Version = viewModel.Version,
                ProductOrCategory = viewModel.ProductOrCategory,
                ItemId = viewModel.ItemId,
                ImageUrl = viewModel.ImageUrl
            };
            if (viewModel.Image == null) return carousel;
            carousel.Image = viewModel.Image;
            return carousel;
        }

        public override CarouselViewModel Map(Carousel entity)
        {
            var carouselViewModel = new CarouselViewModel
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                Active = entity.Active,
                WebActive = entity.WebActive,
                Version = entity.Version,
                ProductOrCategory = entity.ProductOrCategory,
                ItemId = entity.ItemId,
                ImageUrl = entity.ImageUrl
            };
            if (entity.Image == null) return carouselViewModel;
            carouselViewModel.Image = entity.Image;
            return carouselViewModel;
        }
    }
}
