using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class CountryQueryProcessor : QueryBase<Country>, ICountryQueryProcessor
    {
        public CountryQueryProcessor(IUserSession userSession,IDbContext dbContext):base(userSession,dbContext)
        {
                
        }
        public Country ActivateCountry(int id)
        {
            var original = GetValidCountry(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<Country>().Update(original);
            _dbContext.SaveChanges();

            return original;
        }

        public bool Delete(int countryId)
        {
            var country = GetCountry(countryId);
            ValidateAuthorization(country);
            var result = 0;
            if (country == null) return result > 0;
            country.Active = false;
            _dbContext.Set<Country>().Update(country);
            result = _dbContext.SaveChanges();

            return result > 0;
        }

        public bool Exists(Expression<Func<Country, bool>> where)
        {
          return _dbContext.Set<Country>().Any(@where);
        }

        public IQueryable<Country> GetActiveCountries()
        {
            return _dbContext.Set<Country>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }

        public IQueryable<Country> GetAllCountries()
        {
            return _dbContext.Set<Country>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
        }

        public Country[] GetCountry(Expression<Func<Country, bool>> where = null)
        {
            var query = _dbContext.Set<Country>().Where(FilterByActiveTrueAndCompany);
            query=@where==null?query:query.Where(@where);
            var enumerable = query.ToArray();
            return enumerable;

        }

        public Country GetCountry(int countryId)
        {
            var country = _dbContext.Set<Country>().FirstOrDefault(d => d.Id == countryId);
            return country;
        }

        public CountryViewModel GetCountryViewModel(int id)
        {
            var country = _dbContext.Set<Country>().Single(s => s.Id == id);
            var mapper = new CountryToCountryViewModelMapper();
            return mapper.Map(country);
        }

        public IQueryable<Country> GetDeletedCountries()
        {
            return _dbContext.Set<Country>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active==false);
        }

        public Country Save(Country country)
        {
            country.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Country>().Add(country);
            return country;
        }

        public int SaveAll(List<Country> countries)
        {
          _dbContext.Set<Country>().AddRange(countries);
         return _dbContext.SaveChanges();
        }

        public void SaveAllCountry(List<Country> countries)
        {
            _dbContext.Set<Country>().AddRange(countries);
            _dbContext.SaveChanges();
        }

        public Country Update(Country country)
        {
            var original = GetValidCountry(country.Id);
            ValidateAuthorization(country);
            CheckVersionMismatch(country,original);

            original.Name = country.Name;
            original.Code = country.Code;
            original.CompanyId = country.CompanyId;
            original.Active = country.Active;

            _dbContext.Set<Country>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual Country GetValidCountry(int countryId)
        {
            var country = _dbContext.Set<Country>().FirstOrDefault(sc => sc.Id == countryId);
            if (country == null)
                throw new RootObjectNotFoundException("Country not found");
            return country;
        }
    }
}
