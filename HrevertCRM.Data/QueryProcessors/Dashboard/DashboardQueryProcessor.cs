using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Security;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class DashboardQueryProcessor : QueryBase<Dashboard>, IDashboardQueryProcessor
    {
        public DashboardQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public Dashboard GetDashboardValues(int fiscalYearId)
        {
            var dashboard = new Dashboard
            {
                TotalUser = _dbContext.Set<ApplicationUser>().Count(x => x.CompanyId == LoggedInUser.CompanyId && x.Active),
                TotalProduct = _dbContext.Set<Product>().Count(x => x.CompanyId == LoggedInUser.CompanyId && x.Active),
                TotalOrders = _dbContext.Set<SalesOrder>().Count(x => x.CompanyId == LoggedInUser.CompanyId && x.Active),
                TotalSales = _dbContext.Set<SalesOrder>().Count(x => x.CompanyId == LoggedInUser.CompanyId && x.FullyPaid && x.Active),
            };

            var fiscalPeriods = _dbContext.Set<FiscalPeriod>()
                .Include(x => x.SalesOrders)
                .Where(x => x.FiscalYearId == fiscalYearId && x.Active && x.CompanyId == LoggedInUser.CompanyId);

            if (fiscalPeriods == null || !fiscalPeriods.Any()) return dashboard;

            dashboard.OrderFiscalPeriodWise = new List<OrderFiscalPeriodWise>();
            dashboard.SalesFiscalPeriodWise = new List<SalesFiscalPeriodWise>();

            foreach (var fiscalPeriod in fiscalPeriods)
            {
                var fiscalPeriodName = fiscalPeriod.Name;
                dashboard.OrderFiscalPeriodWise
                    .Add(new OrderFiscalPeriodWise
                        {
                            FiscalPeriodName = fiscalPeriodName,
                            OrderCount = fiscalPeriod
                            .SalesOrders
                            .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active)
                            .ToList()
                            .Count
                        }
                    );
            }
            foreach (var fiscalPeriod in fiscalPeriods)
            {
                dashboard.SalesFiscalPeriodWise
                    .Add(new SalesFiscalPeriodWise
                        {
                            FiscalPeriodName = fiscalPeriod.Name,
                            SalesCount = fiscalPeriod
                            .SalesOrders
                            .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.FullyPaid && x.Active)
                            .ToList()
                            .Count
                        }
                    );
            }
            return dashboard;
        }
    }
}
