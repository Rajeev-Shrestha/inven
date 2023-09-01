using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IRetailerQueryProcessor
    {
        List<int> GetRetailers(int distributerId);
        List<int> GetDistributors(int retailerId);
        void Save(RetailerViewModel retailerViewModel);
        void SaveAllRetailers(List<Retailer> retailersOfDistributor);
        bool Delete(int id);
        void Delete(int distibutorId, int retailerId);
    }
}
