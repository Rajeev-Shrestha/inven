using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Company;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;
using HrevertCRM.Entities.Enumerations;
using HrevertCRM.Data.Common;

namespace HrevertCRM.Data.QueryProcessors
{
    public class CompanyWebSettingQueryProcessor : QueryBase<CompanyWebSetting>, ICompanyWebSettingQueryProcessor
    {
        public CompanyWebSettingQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public int SaveAll(List<CompanyWebSetting> companies)
        {
            //companies.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<CompanyWebSetting>().AddRange(companies);
            return _dbContext.SaveChanges();
        }

        public int GetSerialNo(int companyId)
        {
            return _dbContext.Set<CompanyWebSetting>()
                .Where(x => x.CompanyId == companyId)
                .Select(x => x.CustomerSerialNo)
                .FirstOrDefault();
        }

        public CompanyWebSetting Get(int companyId)
        {
            if (companyId == 0)
            {
                companyId = LoggedInUser.CompanyId;
            }
            return _dbContext.Set<CompanyWebSetting>().AsNoTracking().SingleOrDefault(x => x.CompanyId == companyId && x.Active);
        }

        public CompanyWebSetting GetCompanyWebSetting()
        {
            var companyId = LoggedInUser.CompanyId;
            return _dbContext.Set<CompanyWebSetting>().AsNoTracking().SingleOrDefault(x => x.CompanyId == companyId && x.Active);
        }
        public CompanyWebSetting Update(CompanyWebSetting companyWebSetting)
        {
            var original = GetValidCompanyWebSetting(companyWebSetting.Id);
            ValidateAuthorization(companyWebSetting);
            CheckVersionMismatch(companyWebSetting, original); //TODO: to test this method comment this out.

            original.AllowGuest = companyWebSetting.AllowGuest;
            original.ShippingCalculationType = companyWebSetting.ShippingCalculationType;
            original.DiscountCalculationType = companyWebSetting.DiscountCalculationType;
            original.SalesOrderCode = companyWebSetting.SalesOrderCode;
            original.PurchaseOrderCode = companyWebSetting.PurchaseOrderCode;
            original.SalesRepId = companyWebSetting.SalesRepId;
            original.WebActive = companyWebSetting.WebActive;
            original.Active = companyWebSetting.Active;
            original.AllowGuest = companyWebSetting.AllowGuest;
            original.CustomerSerialNo = companyWebSetting.CustomerSerialNo;
            original.VendorSerialNo = companyWebSetting.VendorSerialNo;
            original.IsEstoreInitialized = companyWebSetting.IsEstoreInitialized;
            original.CompanyId = companyWebSetting.CompanyId;
            original.PaymentMethodId = companyWebSetting.PaymentMethodId;
            original.DeliveryMethodId = companyWebSetting.DeliveryMethodId;

            _dbContext.Set<CompanyWebSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual CompanyWebSetting GetValidCompanyWebSetting(int companyWebSettingId)
        {
            var company = _dbContext.Set<CompanyWebSetting>().FirstOrDefault(sc => sc.Id == companyWebSettingId);
            if (company == null)
            {
                throw new RootObjectNotFoundException(CompanyConstants.CompanyWebSettingQueryProcessorConstants.CompanyWebSettingNotFound);
            }
            return company;
        }

        public CompanyWebSetting Save(CompanyWebSetting companyWebSetting)
        {
            companyWebSetting.SalesRepId = LoggedInUser.Id;
            companyWebSetting.CompanyId = LoggedInUser.CompanyId;
            companyWebSetting.Active = true;
            _dbContext.Set<CompanyWebSetting>().Add(companyWebSetting);
            _dbContext.SaveChanges();
            return companyWebSetting;
        }

        public bool Exists(Expression<Func<CompanyWebSetting, bool>> @where)
        {
            return _dbContext.Set<CompanyWebSetting>().Any(@where);
        }

        public bool Delete(int id)
        {
            var doc = Get(id);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<CompanyWebSetting>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool CheckIfEstoreIsInitialized(string userId)
        {
            var companyId = _dbContext.Set<ApplicationUser>().FirstOrDefault(x => x.Id == userId && x.Active).CompanyId;
            if (companyId == 0) return false;
            var isInitialized =
                _dbContext.Set<CompanyWebSetting>().FirstOrDefault(x => x.CompanyId == companyId && x.Active).IsEstoreInitialized;
            return isInitialized;
        }

        public CompanyWebSetting SaveCompanyWebSetting(CompanyWebSetting companyWebSetting)
        {
            companyWebSetting.SalesRepId = LoggedInUser.Id;
            _dbContext.Set<CompanyWebSetting>().Add(companyWebSetting);
            _dbContext.SaveChanges();
            return companyWebSetting;
        }

        public IQueryable<ShippingCalculationTypes> GetShippingDiscountCalculationTypes()
        {
            return _dbContext.Set<ShippingCalculationTypes>().Where(p => p.CompanyId == LoggedInUser.CompanyId && p.Active);
        }

        public IQueryable<DiscountCalculationTypes> GetDiscountCalculationTypes()
        {
            return _dbContext.Set<DiscountCalculationTypes>().Where(p => p.CompanyId == LoggedInUser.CompanyId && p.Active);
        }

        public CompanyWebSetting ActivateCompanyWebSetting(int id)
        {
            var original = GetValidCompanyWebSetting(id);
            ValidateAuthorization(original);
            original.Active = true;
            _dbContext.Set<CompanyWebSetting>().AsNoTracking();
            _dbContext.Set<CompanyWebSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public CompanyWebSetting CheckIfDeletedCompanyWebSettingExists()
        {
            var companyWebSetting =
                _dbContext.Set<CompanyWebSetting>()
                    .FirstOrDefault(
                        x =>
                            x.CompanyId == LoggedInUser.CompanyId && (x.Active || x.Active == false));
            return companyWebSetting;
        }
    }
}
