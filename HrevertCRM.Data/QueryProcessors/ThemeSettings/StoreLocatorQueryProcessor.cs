using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Hrevert.Common.Security;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class StoreLocatorQueryProcessor : QueryBase<StoreLocator>, IStoreLocatorQueryProcessor
    {
        private readonly IHostingEnvironment _env;
        public StoreLocatorQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }

        public StoreLocator GetStoreLocator(int storeLocatorId)
        {
            var store = _dbContext.Set<StoreLocator>().FirstOrDefault(x => x.Id == storeLocatorId && x.CompanyId==LoggedInUser.CompanyId && x.Active);
            return store;
        }

        public StoreLocator Save(StoreLocator storeLocator)
        {
            _dbContext.Set<StoreLocator>().Add(storeLocator);
            _dbContext.SaveChanges();
            return storeLocator;
        }

        public void SaveListOfStoreLocators(List<StoreLocator> storeLocators)
        {
            storeLocators.ForEach(storeLocator => storeLocator.CompanyId = LoggedInUser.CompanyId);
            storeLocators.ForEach(storeLocator => storeLocator.Active = true);
            _dbContext.Set<StoreLocator>().AddRange(storeLocators);
            _dbContext.SaveChanges();
        }

        public StoreLocator Update(StoreLocator storeLocator)
        {
            var original = GetStoreLocator(storeLocator.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(storeLocator, original);
            //pass value to original
            original.Name = storeLocator.Name;
            original.Phone = storeLocator.Phone;
            original.Fax = storeLocator.Fax;
            original.MobileNumber = storeLocator.MobileNumber;
            original.Email = storeLocator.Email;
            original.Longitude = storeLocator.Longitude;
            original.Latitude = storeLocator.Latitude;
            original.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<StoreLocator>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public List<StoreLocator> GetStoreLocatorsByHeaderSettingId(int headerSettingId)
        {
           return _dbContext.Set<StoreLocator>().Where(x => x.HeaderSettingId == headerSettingId && x.CompanyId == LoggedInUser.CompanyId && x.Active).ToList();
        }

        public void UpdateListOfStoreLocators(List<StoreLocator> storeLocators)
        {
            storeLocators.ForEach(storeLocator => storeLocator.CompanyId = LoggedInUser.CompanyId);
            storeLocators.ForEach(storeLocator => storeLocator.Active = true);
            foreach (var storeLocator in storeLocators)
            {
                if (storeLocator.Id == 0) Save(storeLocator);
                else Update(storeLocator);
            }
        }
    }
}
