using System.Collections.Generic;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IBugLoggerQueryProcessor
    {
        BugLogger Save(BugLogger bug);
        BugLogger Update(BugLogger bug);
        BugLogger Get(int bugId);
        List<BugLogger> GetAllBugs();
    }
}
