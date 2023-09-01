using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class PurchaseOrdersStatusQueryProcessor : QueryBase<PurchaseOrdersStatus>, IPurchaseOrdersStatusQueryProcessor
    {
        public PurchaseOrdersStatusQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public PurchaseOrdersStatus Update(PurchaseOrdersStatus purchaseOrdersStatus)
        {
            var original = GetValidPurchaseOrdersStatus(purchaseOrdersStatus.Id);
            ValidateAuthorization(purchaseOrdersStatus);
            CheckVersionMismatch(purchaseOrdersStatus, original);

            original.Value = purchaseOrdersStatus.Value;
            original.Active = purchaseOrdersStatus.Active;
            original.CompanyId = purchaseOrdersStatus.CompanyId;

            _dbContext.Set<PurchaseOrdersStatus>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual PurchaseOrdersStatus GetValidPurchaseOrdersStatus(int purchaseOrdersStatusId)
        {
            var purchaseOrdersStatus = _dbContext.Set<PurchaseOrdersStatus>().FirstOrDefault(sc => sc.Id == purchaseOrdersStatusId);
            if (purchaseOrdersStatus == null)
            {
                throw new RootObjectNotFoundException("Purchase Orders Status not found");
            }
            return purchaseOrdersStatus;
        }
        public PurchaseOrdersStatus GetPurchaseOrdersStatus(int purchaseOrdersStatusId)
        {
            var purchaseOrdersStatus = _dbContext.Set<PurchaseOrdersStatus>().FirstOrDefault(d => d.Id == purchaseOrdersStatusId);
            return purchaseOrdersStatus;
        }
        public void SaveAllPurchaseOrdersStatus(List<PurchaseOrdersStatus> purchaseOrdersStatus)
        {
            _dbContext.Set<PurchaseOrdersStatus>().AddRange(purchaseOrdersStatus);
            _dbContext.SaveChanges();
        }
        public PurchaseOrdersStatus Save(PurchaseOrdersStatus purchaseOrdersStatus)
        {
            purchaseOrdersStatus.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<PurchaseOrdersStatus>().Add(purchaseOrdersStatus);
            _dbContext.SaveChanges();
            return purchaseOrdersStatus;
        }
        public int SaveAll(List<PurchaseOrdersStatus> purchaseOrdersStatus)
        {
            _dbContext.Set<PurchaseOrdersStatus>().AddRange(purchaseOrdersStatus);
            return _dbContext.SaveChanges();
        }
        public PurchaseOrdersStatus ActivatePurchaseOrdersStatus(int id)
        {
            var original = GetValidPurchaseOrdersStatus(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<PurchaseOrdersStatus>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public PurchaseOrderStatusViewModel GetPurchaseOrdersStatusViewModel(int id)
        {
            var purchaseOrdersStatus = _dbContext.Set<PurchaseOrdersStatus>().Single(s => s.Id == id);
            var mapper = new PurchaseOrderStatusToPurchaseOrderStatusViewModelMapper();
            return mapper.Map(purchaseOrdersStatus);
        }
        public bool Delete(int purchaseOrdersStatusId)
        {
            var doc = GetPurchaseOrdersStatus(purchaseOrdersStatusId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<PurchaseOrdersStatus>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<PurchaseOrdersStatus, bool>> @where)
        {
            return _dbContext.Set<PurchaseOrdersStatus>().Any(@where);
        }
        public PurchaseOrdersStatus[] GetPurchaseOrdersStatus(Expression<Func<PurchaseOrdersStatus, bool>> @where = null)
        {
            var query = _dbContext.Set<PurchaseOrdersStatus>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<PurchaseOrdersStatus> GetActivePurchaseOrdersStatus()
        {
            var res = _dbContext.Set<PurchaseOrdersStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
            return res;
        }
        public IQueryable<PurchaseOrdersStatus> GetDeletedPurchaseOrdersStatus()
        {
            return _dbContext.Set<PurchaseOrdersStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<PurchaseOrdersStatus> GetAllPurchaseOrdersStatus()
        {
            var result = _dbContext.Set<PurchaseOrdersStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
