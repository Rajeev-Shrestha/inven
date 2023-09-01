using HrevertCRM.Data;
using HrevertCRM.Data.QueryProcessors;
using Microsoft.Extensions.Logging;

namespace HrevertCRM.Web.Api
{
    public class ProductControllerDependancies : IProductControllerDependancies
    {
        public ProductControllerDependancies(IProductQueryProcessor productQueryProcessor, IProductInCategoryQueryProcessor productInCategoryQueryProcessor, IProductCategoryQueryProcessor productCategoryQueryProcessor, IDbContext dataContext, ILoggerFactory factory)
        {
            ProductQueryProcessor = productQueryProcessor;
            ProductInCategoryQueryProcessor = productInCategoryQueryProcessor;
            ProductCategoryQueryProcessor = productCategoryQueryProcessor;
            DataContext = dataContext;
            Factory = factory;
        }
        public IProductQueryProcessor ProductQueryProcessor { get; }
        public IProductInCategoryQueryProcessor ProductInCategoryQueryProcessor { get; }
        public IProductCategoryQueryProcessor ProductCategoryQueryProcessor { get; }
        public IDbContext DataContext { get; }
        public ILoggerFactory Factory { get; }
    }
}
