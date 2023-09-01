using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Data.Mapper;
using Xunit;
using HrevertCRM.Entities;

namespace ViewModels.Test
{
    public class TestingCarouselAndCarouselViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_Carousel_And_CarouselViewModel(int id)
        {
            var vm = new Carousel()
            {
                Id = 1,
                ItemId = 1,
                ImageUrl = "c:/users/download/ram.jpg",
                WebActive = true,
                CompanyId = 1
            };
            var mappedCarouselVm = new CarouselToCarouselViewModelMapper().Map(vm);
            var carousel = new CarouselToCarouselViewModelMapper().Map(mappedCarouselVm);

            //Test Carousel and mapped Carousel are same
            var res = true;

            PropertyInfo[] mappedproperties = carousel.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(carousel) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(carousel) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(carousel).Equals(propertyValuePair.GetValue(vm));
                    if (!res)
                    {
                        break;
                    }
                }

            }
            Assert.True(res);

        }
    }
}
