using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IDescriptionTypesQueryProcessor
    {
        DescriptionTypes Update(DescriptionTypes descriptionTypes);
        DescriptionTypes GetValidDescriptionTypes(int descriptionTypesId);
        DescriptionTypes GetDescriptionTypes(int descriptionTypesId);
        void SaveAllDescriptionTypes(List<DescriptionTypes> descriptionTypes);
        DescriptionTypes Save(DescriptionTypes descriptionTypes);
        int SaveAll(List<DescriptionTypes> descriptionTypes);
        DescriptionTypes ActivateDescriptionTypes(int id);
        DescriptionTypeViewModel GetDescriptionTypesViewModel(int id);
        bool Delete(int descriptionTypesId);
        bool Exists(Expression<Func<DescriptionTypes, bool>> @where);
        DescriptionTypes[] GetDescriptionTypes(Expression<Func<DescriptionTypes, bool>> @where = null);
        IQueryable<DescriptionTypes> GetActiveDescriptionTypes();
        IQueryable<DescriptionTypes> GetDeletedDescriptionTypes();
        IQueryable<DescriptionTypes> GetAllDescriptionTypes();
    }
}