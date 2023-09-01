using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ICarouselQueryProcessor
    {
        Carousel Update(Carousel carousel);
        Carousel GetCarousel(int carouselId);
        CarouselViewModel GetCarouselViewModel(int carouselId);
        void SaveAllCarousel(List<Carousel> carousels);
        bool Delete(int carouselId);
        bool Exists(Expression<Func<Carousel, bool>> where);
        Carousel ActivateCarousel(int id);
        Carousel[] GetCarousels(Expression<Func<Carousel, bool>> where = null);
        int GetCompanyIdByCarouselId(int carouselId);
        Carousel Save(Carousel carousel);
        string SaveImage(Carousel savedCarousel, Image image);
        IQueryable<Carousel> SearchCarousels(bool active, string searchText);
    }
}