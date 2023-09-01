using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Xunit;

namespace ViewModels.Test
{
    public class TestingJournalMasterAndJournalMasterViewModel
    {
        [Theory]
        [InlineData(1)]
        public void Testing_JournalMaster_And_JournalMasterViewModel(int id)
        {
            var vm = new JournalMaster()
            {
                Id = 1,
                Closed = false,
                Description = "ABC",
                Debit = 231,
                Credit = 2313,
                FiscalPeriodId = 1,
                Posted = false,
                Note = "Test 2",
                PostedDate = Convert.ToDateTime("2016/11/11"),
                Printed = false,
                Cancelled = false,
                IsSystem = false,
                WebActive = true,
                CompanyId = 1
            };
            var mappedJournalMasterVm = new HrevertCRM.Data.Mapper.JournalMasterToJournalMasterViewModelMapper().Map(vm);
            var journalMaster = new HrevertCRM.Data.Mapper.JournalMasterToJournalMasterViewModelMapper().Map(mappedJournalMasterVm);

            //Test JournalMaster and mapped JournalMaster are same
            var res = true;

            PropertyInfo[] mappedproperties = journalMaster.GetType().GetProperties();
            foreach (var propertyValuePair in mappedproperties)
            {
                if (propertyValuePair.CanWrite)
                {
                    if (propertyValuePair.GetValue(journalMaster) == null && propertyValuePair.GetValue(vm) == null)
                    {
                        break;
                    }
                    if (propertyValuePair.GetValue(journalMaster) == null)
                    {
                        res = false;
                        break;

                    }


                    res = propertyValuePair.GetValue(journalMaster).Equals(propertyValuePair.GetValue(vm));
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
