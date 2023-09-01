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
    public class PurchaseOrderTypesQueryProcessor : QueryBase<PurchaseOrderTypes>, IPurchaseOrderTypesQueryProcessor
    {
        public PurchaseOrderTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public PurchaseOrderTypes Update(PurchaseOrderTypes purchaseOrderTypes)
        {
            var original = GetValidPurchaseOrderTypes(purchaseOrderTypes.Id);
            ValidateAuthorization(purchaseOrderTypes);
            CheckVersionMismatch(purchaseOrderTypes, original);

            original.Value = purchaseOrderTypes.Value;
            original.Active = purchaseOrderTypes.Active;
            original.CompanyId = purchaseOrderTypes.CompanyId;

            _dbContext.Set<PurchaseOrderTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual PurchaseOrderTypes GetValidPurchaseOrderTypes(int purchaseOrderTypesId)
        {
            var purchaseOrderTypes = _dbContext.Set<PurchaseOrderTypes>().FirstOrDefault(sc => sc.Id == purchaseOrderTypesId);
            if (purchaseOrderTypes == null)
            {
                throw new RootObjectNotFoundException("Purchase Order Types not found");
            }
            return purchaseOrderTypes;
        }
        public PurchaseOrderTypes GetPurchaseOrderTypes(int purchaseOrderTypesId)
        {
            var purchaseOrderTypes = _dbContext.Set<PurchaseOrderTypes>().FirstOrDefault(d => d.Id == purchaseOrderTypesId);
            return purchaseOrderTypes;
        }
        public void SaveAllPurchaseOrderTypes(List<PurchaseOrderTypes> purchaseOrderTypes)
        {
            _dbContext.Set<PurchaseOrderTypes>().AddRange(purchaseOrderTypes);
            _dbContext.SaveChanges();
        }
        public PurchaseOrderTypes Save(PurchaseOrderTypes purchaseOrderTypes)
        {
            purchaseOrderTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<PurchaseOrderTypes>().Add(purchaseOrderTypes);
            _dbContext.SaveChanges();
            return purchaseOrderTypes;
        }
        public int SaveAll(List<PurchaseOrderTypes> purchaseOrderTypes)
        {
            _dbContext.Set<PurchaseOrderTypes>().AddRange(purchaseOrderTypes);
            return _dbContext.SaveChanges();
        }
        public PurchaseOrderTypes ActivatePurchaseOrderTypes(int id)
        {
            var original = GetValidPurchaseOrderTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<PurchaseOrderTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public PurchaseOrderTypeViewModel GetPurchaseOrderTypesViewModel(int id)
        {
            var purchaseOrderTypes = _dbContext.Set<PurchaseOrderTypes>().Single(s => s.Id == id);
            var mapper = new PurchaseOrderTypeToPurchaseOrderTypeViewModelMapper();
            return mapper.Map(purchaseOrderTypes);
        }
        public bool Delete(int purchaseOrderTypesId)
        {
            var doc = GetPurchaseOrderTypes(purchaseOrderTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<PurchaseOrderTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<PurchaseOrderTypes, bool>> @where)
        {
            return _dbContext.Set<PurchaseOrderTypes>().Any(@where);
        }
        public PurchaseOrderTypes[] GetPurchaseOrderTypes(Expression<Func<PurchaseOrderTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<PurchaseOrderTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<PurchaseOrderTypes> GetActivePurchaseOrderTypes()
        {
            return _dbContext.Set<PurchaseOrderTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<PurchaseOrderTypes> GetDeletedPurchaseOrderTypes()
        {
            return _dbContext.Set<PurchaseOrderTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<PurchaseOrderTypes> GetAllPurchaseOrderTypes()
        {
            var result = _dbContext.Set<PurchaseOrderTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
