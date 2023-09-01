using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hrevert.Common.Enums;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingPaymentTermAndPaymentTermViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_PaymentTerm_And_PaymentTermViewModel(int id)
        {
            var vm = new PaymentTerm()
            {
                Id = 1,
                TermCode = "Test 1",
                Description = "Test 2",
                WebActive = true,
                DueDateType = DueDateType.EndOfMonth,
                DueDateValue = 1,
                DiscountValue = 121,
                DiscountDays = 11,
                DueType = DueType.CashOnDelivery,
                DiscountType = PaymentDiscountType.EndOfMonth,
                CompanyId = 1
            };
            var mappedPaymentTermVm = new HrevertCRM.Data.Mapper.PaymentTermToPaymentTermViewModelMapper().Map(vm);
            var paymentTerm = new HrevertCRM.Data.Mapper.PaymentTermToPaymentTermViewModelMapper().Map(mappedPaymentTermVm);

            //Test PaymentTerm and mappedPaymentTerm are same
            var res = true;

            PropertyInfo[] mappedproperties = paymentTerm.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(paymentTerm) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(paymentTerm) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(paymentTerm).Equals(propertyValuePair.GetValue(vm));
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
