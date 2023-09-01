using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class TaxCalculationTypesQueryProcessor : QueryBase<TaxCalculationTypes>, ITaxCalculationTypesQueryProcessor
    {
        public TaxCalculationTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public TaxCalculationTypes Update(TaxCalculationTypes taxCalculationTypes)
        {
            var original = GetValidTaxCalculationTypes(taxCalculationTypes.Id);
            ValidateAuthorization(taxCalculationTypes);
            CheckVersionMismatch(taxCalculationTypes, original);

            original.Value = taxCalculationTypes.Value;
            original.Active = taxCalculationTypes.Active;
            original.CompanyId = taxCalculationTypes.CompanyId;

            _dbContext.Set<TaxCalculationTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual TaxCalculationTypes GetValidTaxCalculationTypes(int taxCalculationTypesId)
        {
            var taxCalculationTypes = _dbContext.Set<TaxCalculationTypes>().FirstOrDefault(sc => sc.Id == taxCalculationTypesId);
            if (taxCalculationTypes == null)
            {
                throw new RootObjectNotFoundException("Tax Calculation Types not found");
            }
            return taxCalculationTypes;
        }
        public TaxCalculationTypes GetTaxCalculationTypes(int taxCalculationTypesId)
        {
            var taxCalculationTypes = _dbContext.Set<TaxCalculationTypes>().FirstOrDefault(d => d.Id == taxCalculationTypesId);
            return taxCalculationTypes;
        }
        public void SaveAllTaxCalculationTypes(List<TaxCalculationTypes> taxCalculationTypes)
        {
            _dbContext.Set<TaxCalculationTypes>().AddRange(taxCalculationTypes);
            _dbContext.SaveChanges();
        }
        public TaxCalculationTypes Save(TaxCalculationTypes taxCalculationTypes)
        {
            taxCalculationTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<TaxCalculationTypes>().Add(taxCalculationTypes);
            _dbContext.SaveChanges();
            return taxCalculationTypes;
        }
        public int SaveAll(List<TaxCalculationTypes> taxCalculationTypes)
        {
            _dbContext.Set<TaxCalculationTypes>().AddRange(taxCalculationTypes);
            return _dbContext.SaveChanges();
        }
        public TaxCalculationTypes ActivateTaxCalculationTypes(int id)
        {
            var original = GetValidTaxCalculationTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<TaxCalculationTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public TaxCalculationTypeViewModel GetTaxCalculationTypesViewModel(int id)
        {
            var taxCalculationTypes = _dbContext.Set<TaxCalculationTypes>().Single(s => s.Id == id);
            var mapper = new TaxCalculationTypeToTaxCalculationTypeViewModelMapper();
            return mapper.Map(taxCalculationTypes);
        }
        public bool Delete(int taxCalculationTypesId)
        {
            var doc = GetTaxCalculationTypes(taxCalculationTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<TaxCalculationTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<TaxCalculationTypes, bool>> @where)
        {
            return _dbContext.Set<TaxCalculationTypes>().Any(@where);
        }
        public TaxCalculationTypes[] GetTaxCalculationTypes(Expression<Func<TaxCalculationTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<TaxCalculationTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<TaxCalculationTypes> GetActiveTaxCalculationTypes()
        {
            return _dbContext.Set<TaxCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<TaxCalculationTypes> GetDeletedTaxCalculationTypes()
        {
            return _dbContext.Set<TaxCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<TaxCalculationTypes> GetAllTaxCalculationTypes()
        {
            var result = _dbContext.Set<TaxCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }

       
    }
}
