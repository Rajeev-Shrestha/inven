using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace HrevertCRM.Data.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1
    {

        [Fact]
        public void UniTest1()
        {
            var vm = new ProductViewModel()
            {
                Id = 1,Categories = new List < int >{ 1,3},Code = "A",CommissionRate = 1, Commissionable = true,Name = "A Product",ProductDescription = "Test",QuantityOnHand = 5,QuantityOnOrder = 5,UnitPrice = 10, WebActive = true
            };
            var mappedProduct = new Mapper.ProductToProductViewModelMapper().Map(vm);
            var product= new Product();
            Type prodType = vm.GetType();
            PropertyInfo[] properties = prodType.GetProperties();
            foreach (var propertyValuePair in properties)
            {
                if (propertyValuePair.CanWrite)
                    properties.Single(x => x.Name == propertyValuePair.Name)
                    .SetValue(product, propertyValuePair.GetValue(vm));
            }
            //Test Product and mappedProduct are same

            PropertyInfo[] mappedproperties = mappedProduct.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    Assert.True(propertyValuePair.GetValue(product) == propertyValuePair.GetValue(mappedProduct));
                }
                  
            }



 
        }
    }
}
