using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ICountryQueryProcessor
    {
        Country Update(Country country);
        Country GetValidCountry(int countryId);
        Country GetCountry(int countryId);
        void SaveAllCountry(List<Country> countries);
        Country Save(Country country);
        int SaveAll(List<Country> countries);
        Country ActivateCountry(int id);
        CountryViewModel GetCountryViewModel(int id);
        bool Delete(int countryId);
        bool Exists(Expression<Func<Country, bool>> @where);
        Country[] GetCountry(Expression<Func<Country, bool>> @where = null);
        IQueryable<Country> GetActiveCountries();
        IQueryable<Country> GetDeletedCountries();
        IQueryable<Country> GetAllCountries();
    }
}
