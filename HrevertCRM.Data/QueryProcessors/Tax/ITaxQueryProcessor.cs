using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data
{
    public interface ITaxQueryProcessor
    {
        Tax Update(Tax tax);
        void SaveAllTax(List<Tax> taxes);
        bool Delete(int taxId);
        bool Exists(Expression<Func<Tax, bool>> @where);
        Tax[] GetTaxes(Expression<Func<Tax, bool>> @where = null);
        Tax Save(Tax tax);
        int SaveAll(List<Tax> taxes);
        Tax ActivateTax(int id);
        TaxViewModel GetTaxViewModel(int id);
        IQueryable<Tax> GetAllActiveTaxes();
        IQueryable<Tax> GetActiveTaxesWithoutPaging();
        IQueryable<Tax> GetActiveTaxesWithoutPaging(int distributorId);
        Tax CheckIfDeletedTaxWithSameTaxCodeExists(string taxCode);
        IQueryable<Tax> SearchTaxes(bool active, string searchText);

        bool DeleteRange(List<int?> taxesId);
    }
}