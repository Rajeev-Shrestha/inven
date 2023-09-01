using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.SalesOrder;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data
{
    public class TaxQueryProcessor : QueryBase<Tax>, ITaxQueryProcessor
    {
        public TaxQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public Tax Update(Tax tax)
        {
            var original = GetValidTax(tax.Id);
            ValidateAuthorization(tax);
            CheckVersionMismatch(tax, original);

            original.TaxCode = tax.TaxCode;
            original.Description = tax.Description;
            original.IsRecoverable = tax.IsRecoverable;
            original.TaxType = tax.TaxType;
            original.TaxRate = tax.TaxRate;
            original.RecoverableCalculationType = tax.RecoverableCalculationType;
            original.Active = tax.Active;
            original.CompanyId = tax.CompanyId;
            original.Version = tax.Version;
            original.WebActive = tax.WebActive;
            original.CompanyId = tax.CompanyId;
            original.Active = tax.Active;
            original.WebActive = tax.WebActive;

            _dbContext.Set<Tax>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual Tax GetValidTax(int taxId)
        {
            var tax = _dbContext.Set<Tax>().FirstOrDefault(sc => sc.Id == taxId);
            if (tax == null)
            {
                throw new RootObjectNotFoundException(SalesOrderConstants.TaxQueryProcessorConstants.TaxNotFound);
            }
            return tax;
        }

        public Tax GetTax(int taxId)
        {
            var tax = _dbContext.Set<Tax>().FirstOrDefault(d => d.Id == taxId);
            return tax;
        }
        public void SaveAllTax(List<Tax> taxes)
        {
            _dbContext.Set<Tax>().AddRange(taxes);
            _dbContext.SaveChanges();
        }

        public Tax Save(Tax tax)
        {
            tax.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<Tax>().Add(tax);
            _dbContext.SaveChanges();
            return tax;
        }

        public int SaveAll(List<Tax> taxes)
        {
            _dbContext.Set<Tax>().AddRange(taxes);
            return _dbContext.SaveChanges();

        }

        public Tax ActivateTax(int id)
        {
            var original = GetValidTax(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<Tax>().Update(original);
            _dbContext.SaveChanges();
            return original;

        }

        public TaxViewModel GetTaxViewModel(int id)
        {
            var tax = _dbContext.Set<Tax>().Single(s => s.Id == id);
            var mapper = new TaxToTaxViewModelMapper();
            return mapper.Map(tax);
        }

        public IQueryable<Tax> GetAllActiveTaxes()
        {
            return _dbContext.Set<Tax>().Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
        }

        public bool Delete(int taxId)
        {
            var doc = GetTax(taxId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<Tax>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<Tax, bool>> where)
        {
            return _dbContext.Set<Tax>().Any(where);
        }

        public Tax[] GetTaxes(Expression<Func<Tax, bool>> where = null)
        {

            var query = _dbContext.Set<Tax>().Where(FilterByActiveTrueAndCompany);
            query = where == null ? query : query.Where(where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public IQueryable<Tax> GetActiveTaxesWithoutPaging()
        {
            var taxes = _dbContext.Set<Tax>().Where(FilterByActiveTrueAndCompany);
            return taxes;
        }

        public IQueryable<Tax> GetActiveTaxesWithoutPaging(int distributorId)
        {
            return _dbContext.Set<Tax>().Where(x => x.CompanyId == distributorId && x.Active);
        }

        public Tax CheckIfDeletedTaxWithSameTaxCodeExists(string taxCode)
        {
            var tax =
                _dbContext.Set<Tax>()
                    .FirstOrDefault(
                        x =>
                            x.TaxCode == taxCode && x.CompanyId == LoggedInUser.CompanyId && (x.Active ||
                            x.Active == false));
            return tax;
        }
        public IQueryable<Tax> SearchTaxes(bool active, string searchText)
        {
            var taxes = _dbContext.Set<Tax>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                taxes = taxes.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? taxes
                : taxes.Where(s => s.TaxCode.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public bool DeleteRange(List<int?> taxesId)
        {
            var taxList = taxesId.Select(taxId => _dbContext.Set<Tax>().FirstOrDefault(x => x.Id == taxId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            taxList.ForEach(x => x.Active = false);
            _dbContext.Set<Tax>().UpdateRange(taxList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
