using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IDashboardQueryProcessor
    {
        Dashboard GetDashboardValues(int fiscalYearId);
    }
}
