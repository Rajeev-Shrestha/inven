using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ITermTypesQueryProcessor
    {
        TermTypes Update(TermTypes termTypes);
        TermTypes GetValidTermTypes(int termTypesId);
        TermTypes GetTermTypes(int termTypesId);
        void SaveAllTermTypes(List<TermTypes> termTypes);
        TermTypes Save(TermTypes termTypes);
        int SaveAll(List<TermTypes> termTypes);
        TermTypes ActivateTermTypes(int id);
        TermTypeViewModel GetTermTypesViewModel(int id);
        bool Delete(int termTypesId);
        bool Exists(Expression<Func<TermTypes, bool>> @where);
        TermTypes[] GetTermTypes(Expression<Func<TermTypes, bool>> @where = null);
        IQueryable<TermTypes> GetActiveTermTypes();
        IQueryable<TermTypes> GetDeletedTermTypes();
        IQueryable<TermTypes> GetAllTermTypes();
    }
}