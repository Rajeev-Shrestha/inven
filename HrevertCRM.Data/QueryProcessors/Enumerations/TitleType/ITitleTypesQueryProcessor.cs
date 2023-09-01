using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ITitleTypesQueryProcessor
    {
        TitleTypes Update(TitleTypes titleTypes);
        TitleTypes GetValidTitleTypes(int titleTypesId);
        TitleTypes GetTitleTypes(int titleTypesId);
        void SaveAllTitleTypes(List<TitleTypes> titleTypes);
        TitleTypes Save(TitleTypes titleTypes);
        int SaveAll(List<TitleTypes> titleTypes);
        TitleTypes ActivateTitleTypes(int id);
        TitleTypeViewModel GetTitleTypesViewModel(int id);
        bool Delete(int titleTypesId);
        bool Exists(Expression<Func<TitleTypes, bool>> @where);
        TitleTypes[] GetTitleTypes(Expression<Func<TitleTypes, bool>> @where = null);
        IQueryable<TitleTypes> GetActiveTitleTypes();
        IQueryable<TitleTypes> GetDeletedTitleTypes();
        IQueryable<TitleTypes> GetAllTitleTypes();
    }
}