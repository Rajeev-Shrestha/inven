using HrevertCRM.Data;
using HrevertCRM.Data.QueryProcessors;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    public interface IProductControllerDependancies
    {
        IProductQueryProcessor ProductQueryProcessor { get; }
        IProductInCategoryQueryProcessor ProductInCategoryQueryProcessor { get; }
        IProductCategoryQueryProcessor ProductCategoryQueryProcessor { get; }
        IDbContext DataContext { get; }
        ILoggerFactory Factory { get; }
    }
}
