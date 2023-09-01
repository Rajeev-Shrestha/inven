using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IMeasureUnitQueryProcessor
    {
        MeasureUnit Update(MeasureUnit measureUnit);
        MeasureUnit GetMeasureUnit(int measureUnitId);
        MeasureUnitViewModel GetMeasureUnitViewModel(int measureUnitId);
        void SaveAll(List<MeasureUnit> measureUnits);
        bool Delete(int measureUnitId);
        bool Exists(Expression<Func<MeasureUnit, bool>> where);
        MeasureUnit ActivateMeasureUnit(int id);
        MeasureUnit[] GetMeasureUnits(Expression<Func<MeasureUnit, bool>> where = null);
        MeasureUnit Save(MeasureUnit measureUnit);
        IQueryable<EntryMethodTypes> GetAllEntryMethodTypes();
        MeasureUnit CheckIfDeletedMeasureUnitWithSameCodeExists(string code);
        MeasureUnit CheckIfDeletedMeasureUnitWithSameNameExists(string name);
        IQueryable<MeasureUnit> SearchMeasureUnits(bool active, string searchText);
        bool DeleteRange(List<int?> measureUnitsId);
       
    }
}