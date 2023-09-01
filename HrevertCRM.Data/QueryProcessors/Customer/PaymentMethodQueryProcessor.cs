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
    public class PaymentMethodQueryProcessor : QueryBase<PaymentMethod>, IPaymentMethodQueryProcessor
    {
        public PaymentMethodQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public PaymentMethod Update(PaymentMethod paymentMethod)
        {
            var original = GetValidPaymentMethod(paymentMethod.Id);
            ValidateAuthorization(paymentMethod);
            CheckVersionMismatch(paymentMethod, original); //TODO this is comment to test this method

            original.MethodCode = paymentMethod.MethodCode;
            original.MethodName = paymentMethod.MethodName;
            original.AccountId = paymentMethod.AccountId;
            original.ReceipentMemo = paymentMethod.ReceipentMemo;
            original.WebActive = paymentMethod.WebActive;
            original.Active = paymentMethod.Active;
            original.CompanyId = paymentMethod.CompanyId;

            _dbContext.Set<PaymentMethod>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public PaymentMethod GetValidPaymentMethod(int paymentMethodId)
        {
            var paymentMethod = _dbContext.Set<PaymentMethod>().FirstOrDefault(sc => sc.Id == paymentMethodId);
            if (paymentMethod == null)
            {
                throw new RootObjectNotFoundException(CustomerConstants.PaymentMethodQueryProcessorConstants.PaymentMethodNotFound);
            }
            return paymentMethod;
        }

        public PaymentMethod GetPaymentMethod(int paymentMethodId)
        {
            var paymentMethod = _dbContext.Set<PaymentMethod>().FirstOrDefault(d => d.Id == paymentMethodId);
            return paymentMethod;
        }

        public void SaveAllPaymentMethod(List<PaymentMethod> paymentMethods)
        {
            _dbContext.Set<PaymentMethod>().AddRange(paymentMethods);
            _dbContext.SaveChanges();
        }

        public PaymentMethod Save(PaymentMethod paymentMethod)
        {
            paymentMethod.Active = true;
            paymentMethod.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<PaymentMethod>().Add(paymentMethod);
            _dbContext.SaveChanges();
            return paymentMethod;
        }

        public int SaveAll(List<PaymentMethod> paymentMethods)
        {
            _dbContext.Set<PaymentMethod>().AddRange(paymentMethods);
            return _dbContext.SaveChanges();
        }

        public bool Delete(int paymentMethodId)
        {
            var doc = GetPaymentMethod(paymentMethodId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<PaymentMethod>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<PaymentMethod, bool>> @where)
        {
            return _dbContext.Set<PaymentMethod>().Any(@where);
        }

        public QueryResult<PaymentMethod> GetPaymentMethods(PagedDataRequest requestInfo, Expression<Func<PaymentMethod, bool>> @where = null)
        {
            var query = _dbContext.Set<PaymentMethod>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<PaymentMethod>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public PaymentMethod[] GetPaymentMethods(Expression<Func<PaymentMethod, bool>> @where = null)
        {
            var query = _dbContext.Set<PaymentMethod>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public PaymentMethod ActivatePaymentMethod(int id)
        {
            var original = GetValidPaymentMethod(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<PaymentMethod>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public PaymentMethod CheckIfDeletedPaymentMethodWithSameCodeExists(string code)
        {
            var paymentMethod =
                _dbContext.Set<PaymentMethod>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.MethodCode == code && (x.Active ||
                             x.Active == false));
            return paymentMethod;
        }

        public PaymentMethod CheckIfDeletedPaymentMethodWithSameNameExists(string name)
        {
            var paymentMethod =
                _dbContext.Set<PaymentMethod>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.MethodName == name && x.Active ||
                             x.Active == false);
            return paymentMethod;
        }

        public IQueryable<PaymentMethod> SearchPaymentMethods(bool active, string searchText)
        {
            var paymentMethods = _dbContext.Set<PaymentMethod>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                paymentMethods = paymentMethods.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? paymentMethods
                : paymentMethods.Where(s => s.MethodCode.ToUpper().Contains(searchText.ToUpper())
                                            || s.MethodName.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public bool DeleteRange(List<int?> paymentMethodsId)
        {
            var paymentMethodList = paymentMethodsId.Select(paymentMethodId => _dbContext.Set<PaymentMethod>().FirstOrDefault(x => x.Id == paymentMethodId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            paymentMethodList.ForEach(x => x.Active = false);
            _dbContext.Set<PaymentMethod>().UpdateRange(paymentMethodList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
