using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ITaxCalculationTypesQueryProcessor
    {
        TaxCalculationTypes Update(TaxCalculationTypes taxCalculationTypes);
        TaxCalculationTypes GetValidTaxCalculationTypes(int taxCalculationTypesId);
        TaxCalculationTypes GetTaxCalculationTypes(int taxCalculationTypesId);
        void SaveAllTaxCalculationTypes(List<TaxCalculationTypes> taxCalculationTypes);
        TaxCalculationTypes Save(TaxCalculationTypes taxCalculationTypes);
        int SaveAll(List<TaxCalculationTypes> taxCalculationTypes);
        TaxCalculationTypes ActivateTaxCalculationTypes(int id);
        TaxCalculationTypeViewModel GetTaxCalculationTypesViewModel(int id);
        bool Delete(int taxCalculationTypesId);
        bool Exists(Expression<Func<TaxCalculationTypes, bool>> @where);
        TaxCalculationTypes[] GetTaxCalculationTypes(Expression<Func<TaxCalculationTypes, bool>> @where = null);
        IQueryable<TaxCalculationTypes> GetActiveTaxCalculationTypes();
        IQueryable<TaxCalculationTypes> GetDeletedTaxCalculationTypes();
        IQueryable<TaxCalculationTypes> GetAllTaxCalculationTypes();
    }
}