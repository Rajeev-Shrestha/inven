using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingPaymentMethodAndPaymentMethodViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_PaymentMethod_And_PaymentMethodViewModel(int id)
        {
            var vm = new PaymentMethod()
            {
                Id = 1,
                MethodCode = "Test 1",
                MethodName = "Test 2",
                AccountId = 1,
                ReceipentMemo = "Test 3",
                WebActive = true,
                CompanyId = 1
            };
            var mappedPaymentMethodVm = new HrevertCRM.Data.Mapper.PaymentMethodToPaymentMethodViewModelMapper().Map(vm);
            var paymentMethod = new HrevertCRM.Data.Mapper.PaymentMethodToPaymentMethodViewModelMapper().Map(mappedPaymentMethodVm);

            //Test PaymentMethod and mappedPaymentMethod are same
            var res = true;

            PropertyInfo[] mappedproperties = paymentMethod.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(paymentMethod) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(paymentMethod) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(paymentMethod).Equals(propertyValuePair.GetValue(vm));
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
