using System;
using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Security;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class RetailerQueryProcessor : QueryBase<Retailer>, IRetailerQueryProcessor
    {
        public RetailerQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public List<int> GetRetailers(int distributorId)
        {
            var retailersId =
                _dbContext.Set<Retailer>().Where(x => x.DistibutorId == distributorId).Select(x => x.RetailerId).ToList();
            return retailersId;
        }

        public List<int> GetDistributors(int retailerId)
        {
            var distributersId =
                _dbContext.Set<Retailer>().Where(x => x.RetailerId == retailerId).Select(x => x.DistibutorId).ToList();
            return distributersId;
        }

        public void Save(RetailerViewModel retailerViewModel)
        {
            throw new NotImplementedException();
        }

        public void SaveAllRetailers(List<Retailer> retailersOfDistributor)
        {
            retailersOfDistributor.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<Retailer>().AddRange(retailersOfDistributor);
            _dbContext.SaveChanges();
        }

        public bool Delete(int id)
        {
            var doc = GetRetailer(id);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<Retailer>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public void Delete(int distibutorId, int retailerId)
        {
            var doc = GetRetailer(distibutorId, retailerId);
            if (doc == null) return;
            _dbContext.Set<Retailer>().Remove(doc);
            _dbContext.SaveChanges();
        }

        private Retailer GetRetailer(int distibutorId, int retailerId)
        {
            return
                _dbContext.Set<Retailer>()
                    .FirstOrDefault(x => x.DistibutorId == distibutorId && x.RetailerId == retailerId);
        }

        private Retailer GetRetailer(int id)
        {
            return _dbContext.Set<Retailer>().FirstOrDefault(x => x.Id == id);
        }
    }
}

