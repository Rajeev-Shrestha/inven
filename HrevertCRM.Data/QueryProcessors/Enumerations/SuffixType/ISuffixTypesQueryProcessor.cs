using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface ISuffixTypesQueryProcessor
    {
        SuffixTypes Update(SuffixTypes suffixTypes);
        SuffixTypes GetValidSuffixTypes(int suffixTypesId);
        SuffixTypes GetSuffixTypes(int suffixTypesId);
        void SaveAllSuffixTypes(List<SuffixTypes> suffixTypes);
        SuffixTypes Save(SuffixTypes suffixTypes);
        int SaveAll(List<SuffixTypes> suffixTypes);
        SuffixTypes ActivateSuffixTypes(int id);
        SuffixTypeViewModel GetSuffixTypesViewModel(int id);
        bool Delete(int suffixTypesId);
        bool Exists(Expression<Func<SuffixTypes, bool>> @where);
        SuffixTypes[] GetSuffixTypes(Expression<Func<SuffixTypes, bool>> @where = null);
        IQueryable<SuffixTypes> GetActiveSuffixTypes();
        IQueryable<SuffixTypes> GetDeletedSuffixTypes();
        IQueryable<SuffixTypes> GetAllSuffixTypes();
    }
}