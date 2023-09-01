using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;
using System.Linq.Expressions;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class ShippingStatusQueryProcessor:QueryBase<ShippingStatus>,IShippingStatusQueryProcessor
    {
        public ShippingStatusQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public ShippingStatus Update(ShippingStatus shippingStatus)
        {
            var original = GetValidShippingStatus(shippingStatus.Id);
            ValidateAuthorization(shippingStatus);
            CheckVersionMismatch(shippingStatus, original);

            original.Value = shippingStatus.Value;
            original.Active = shippingStatus.Active;
            original.CompanyId = shippingStatus.CompanyId;

            _dbContext.Set<ShippingStatus>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual ShippingStatus GetValidShippingStatus(int ShippingStatusId)
        {
            var ShippingStatus = _dbContext.Set<ShippingStatus>().FirstOrDefault(sc => sc.Id == ShippingStatusId);
            if (ShippingStatus == null)
            {
                throw new RootObjectNotFoundException("Shipping Status not found");
            }
            return ShippingStatus;
        }
        public ShippingStatus GetShippingStatus(int shippingStatusId)
        {
            var shippingStatus = _dbContext.Set<ShippingStatus>().FirstOrDefault(d => d.Id == shippingStatusId);
            return shippingStatus;
        }
        public void SaveAllShippingStatus(List<ShippingStatus> shippingStatus)
        {
            _dbContext.Set<ShippingStatus>().AddRange(shippingStatus);
            _dbContext.SaveChanges();
        }
        public ShippingStatus Save(ShippingStatus shippingStatus)
        {
            shippingStatus.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ShippingStatus>().Add(shippingStatus);
            _dbContext.SaveChanges();
            return shippingStatus;
        }
        public int SaveAll(List<ShippingStatus> ShippingStatus)
        {
            _dbContext.Set<ShippingStatus>().AddRange(ShippingStatus);
            return _dbContext.SaveChanges();
        }
        public ShippingStatus ActivateShippingStatus(int id)
        {
            var original = GetValidShippingStatus(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ShippingStatus>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public ShippingStatusViewModel GetShippingStatusViewModel(int id)
        {
            var ShippingStatus = _dbContext.Set<ShippingStatus>().Single(s => s.Id == id);
            var mapper = new ShippingStatusToShippingStatusViewModelMapper();
            return mapper.Map(ShippingStatus);
        }
        public bool Delete(int ShippingStatusId)
        {
            var doc = GetShippingStatus(ShippingStatusId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ShippingStatus>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<ShippingStatus, bool>> @where)
        {
            return _dbContext.Set<ShippingStatus>().Any(@where);
        }
        public ShippingStatus[] GetShippingStatus(Expression<Func<ShippingStatus, bool>> @where = null)
        {

            var query = _dbContext.Set<ShippingStatus>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<ShippingStatus> GetActiveShippingStatus()
        {
            return _dbContext.Set<ShippingStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<ShippingStatus> GetDeletedShippingStatus()
        {
            return _dbContext.Set<ShippingStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<ShippingStatus> GetAllShippingStatus()
        {
            var result = _dbContext.Set<ShippingStatus>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
