using HrevertCRM.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISourceQueryProcessor
    {
        Source Save(Source source);
        Source Update(Source source);
        int SaveAll(List<Source> sources);
        bool Delete(int sourceId);
        Source GetSourceById(int sourceId);
        IQueryable<Source> GetAll();
        IQueryable<Source> GetAllActive();
    }
}
