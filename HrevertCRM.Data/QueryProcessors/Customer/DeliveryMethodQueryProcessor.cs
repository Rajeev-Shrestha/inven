using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;


namespace HrevertCRM.Data.QueryProcessors
{
    public class DeliveryMethodQueryProcessor : QueryBase<DeliveryMethod>, IDeliveryMethodQueryProcessor
    {
        public DeliveryMethodQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public DeliveryMethod Update(DeliveryMethod deliveryMethod)
        {
            var original = GetValidDeliveryMethod(deliveryMethod.Id);
            ValidateAuthorization(deliveryMethod);
            CheckVersionMismatch(deliveryMethod, original);  //TODO this is comment to test this method
            
            original.DeliveryCode = deliveryMethod.DeliveryCode;
            original.Description = deliveryMethod.Description;
            original.WebActive = deliveryMethod.WebActive;
            original.Active = deliveryMethod.Active;

            _dbContext.Set<DeliveryMethod>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual DeliveryMethod GetValidDeliveryMethod(int deliveryMethodId)
        {
            var deliveryMethod = _dbContext.Set<DeliveryMethod>().FirstOrDefault(sc => sc.Id == deliveryMethodId);
            if (deliveryMethod == null)
            {
                throw new RootObjectNotFoundException(CustomerConstants.DeliveryMethodQueryProcessorConstants.DeliveryMethodNotFound);
            }
            return deliveryMethod;
        }

        public DeliveryMethod GetDeliveryMethod(int deliveryMethodId)
        {
            var deliveryMethod = _dbContext.Set<DeliveryMethod>().FirstOrDefault(d => d.Id == deliveryMethodId);
            return deliveryMethod;
        }

        public void SaveAllDeliveryMethod(List<DeliveryMethod> deliveryMethods)
        {
            _dbContext.Set<DeliveryMethod>().AddRange(deliveryMethods);
            _dbContext.SaveChanges();
        }

        public DeliveryMethod Save(DeliveryMethod deliveryMethod)
        {
            deliveryMethod.CompanyId = LoggedInUser.CompanyId;
            deliveryMethod.Active = true;
            _dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
            _dbContext.SaveChanges();
            return deliveryMethod;
        }

        public int SaveAll(List<DeliveryMethod> deliveryMethods)
        {
            _dbContext.Set<DeliveryMethod>().AddRange(deliveryMethods);
            return _dbContext.SaveChanges();

        }

        public bool Delete(int deliveryMethodId)
        {
            var doc = GetDeliveryMethod(deliveryMethodId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DeliveryMethod>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<DeliveryMethod, bool>> @where)
        {
            return _dbContext.Set<DeliveryMethod>().Any(@where);
        }

        public QueryResult<DeliveryMethod> GetDeliveryMethods(PagedDataRequest requestInfo, Expression<Func<DeliveryMethod, bool>> @where = null)
        {
            var query = _dbContext.Set<DeliveryMethod>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<DeliveryMethod>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public DeliveryMethod[] GetDeliveryMethods(Expression<Func<DeliveryMethod, bool>> @where = null)
        {
            var query = _dbContext.Set<DeliveryMethod>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public DeliveryMethod ActivateDeliveryMethod(int id)
        {
            var original = GetValidDeliveryMethod(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DeliveryMethod>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public DeliveryMethod CheckIfDeletedDeliveryMethodWithSameCodeExists(string code)
        {
            var deliveryMethod =
                _dbContext.Set<DeliveryMethod>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.DeliveryCode == code && (x.Active ||
                             x.Active == false));
            return deliveryMethod;
        }

        public IQueryable<DeliveryMethod> SearchDeliveryMethods(bool active, string searchText)
        {
            var deliveryMethods = _dbContext.Set<DeliveryMethod>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            if (active)
                deliveryMethods = deliveryMethods.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? deliveryMethods
                : deliveryMethods.Where(s => s.DeliveryCode.ToUpper().Contains(searchText.ToUpper()) || s.Description.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public bool DeleteRange(List<int?> deliveryMethodsId)
        {
            var deliverMethodList = deliveryMethodsId.Select(deliveryMethodId => _dbContext.Set<DeliveryMethod>().FirstOrDefault(x => x.Id == deliveryMethodId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            deliverMethodList.ForEach(x => x.Active = false);
            _dbContext.Set<DeliveryMethod>().UpdateRange(deliverMethodList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
