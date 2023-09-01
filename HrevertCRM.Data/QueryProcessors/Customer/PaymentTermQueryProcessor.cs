using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Constants.Customer;
using Hrevert.Common.Enums;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;


namespace HrevertCRM.Data.QueryProcessors
{
    public class PaymentTermQueryProcessor : QueryBase<PaymentTerm>, IPaymentTermQueryProcessor
    {
        public PaymentTermQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public PaymentTerm Update(PaymentTerm paymentTerm) 
        {
            var original = GetValidPaymentTerm(paymentTerm.Id);
            ValidateAuthorization(paymentTerm);
            CheckVersionMismatch(paymentTerm, original);//TODO this is comment to test this method

            original.TermCode = paymentTerm.TermCode;
            original.TermName = paymentTerm.TermName;
            original.Description = paymentTerm.Description;
            original.TermType = paymentTerm.TermType;
            original.DueDateType = paymentTerm.DueDateType;
            original.DueType = paymentTerm.DueType;
            original.DueDateValue = paymentTerm.DueDateValue;
            original.DiscountType = paymentTerm.DiscountType;
            original.DiscountValue = paymentTerm.DiscountValue;
            original.DiscountDays = paymentTerm.DiscountDays;
            original.Active = paymentTerm.Active;
            original.CompanyId = paymentTerm.CompanyId;
            original.WebActive = paymentTerm.WebActive;

            _dbContext.Set<PaymentTerm>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual PaymentTerm GetValidPaymentTerm(int paymentTermId)
        {
            var paymentTerm = _dbContext.Set<PaymentTerm>().FirstOrDefault(sc => sc.Id == paymentTermId);
            if (paymentTerm == null)
            {
                throw new RootObjectNotFoundException(CustomerConstants.PaymentTermQueryProcessorConstants.PaymentTermNotFound);
            }
            return paymentTerm;
        }

        public PaymentTerm GetPaymentTerm(int paymentTermId)
        {
            var paymentTerm = _dbContext.Set<PaymentTerm>().FirstOrDefault(d => d.Id == paymentTermId);
            return paymentTerm;
        }

        public void SaveAllPaymentTerm(List<PaymentTerm> paymentTerms)
        {
            _dbContext.Set<PaymentTerm>().AddRange(paymentTerms);
            _dbContext.SaveChanges();
        }

        public PaymentTerm Save(PaymentTerm paymentTerm)
        {
            paymentTerm.CompanyId = LoggedInUser.CompanyId;
            paymentTerm.Active = true;
            _dbContext.Set<PaymentTerm>().Add(paymentTerm);
            _dbContext.SaveChanges();
            return paymentTerm;
        }
        public int SaveAll(List<PaymentTerm> paymentTerms)
        {
            _dbContext.Set<PaymentTerm>().AddRange(paymentTerms);
            return _dbContext.SaveChanges();
        }

        public bool Delete(int paymentTermId)
        {
            var doc = GetPaymentTerm(paymentTermId);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<PaymentTerm>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public bool Exists(Expression<Func<PaymentTerm, bool>> @where)
        {
            return _dbContext.Set<PaymentTerm>().Any(@where);
        }

        public QueryResult<PaymentTerm> GetPaymentTerms(PagedDataRequest requestInfo, Expression<Func<PaymentTerm, bool>> @where = null)
        {
            var query = _dbContext.Set<PaymentTerm>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            //var enumerable = query as PaymentTerm[] ?? query.ToArray();
            var totalItemCount = query.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(requestInfo.PageNumber, requestInfo.PageSize);
            var docs = query.OrderBy(x => x.DateCreated).Skip(startIndex).Take(requestInfo.PageSize).ToList();
            var queryResult = new QueryResult<PaymentTerm>(docs, totalItemCount, requestInfo.PageSize);
            return queryResult;
        }

        public PaymentTerm[] GetPaymentTerms(Expression<Func<PaymentTerm, bool>> @where = null)
        {
            var query = _dbContext.Set<PaymentTerm>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }

        public PaymentTerm ActivatePaymentTerm(int id)
        {
            var original = GetValidPaymentTerm(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<PaymentTerm>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public List<PaymentTerm> GetOnAccountTerms()
        {
            return
                _dbContext.Set<PaymentTerm>()
                    .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active && x.TermType == TermType.OnAccount)
                    .ToList();
        }

        public PayMethodsInPayTerm SavePayMethodInPayTerms(PayMethodsInPayTerm payMethodsInPayTerm)
        {
            throw new NotImplementedException();
        }

        public List<int> GetPayMethodIdsByTermId(int id)
        {
            var paymentMethodIds =
                _dbContext.Set<PayMethodsInPayTerm>()
                    .Where(x => x.PayTermId == id && x.CompanyId == LoggedInUser.CompanyId && x.Active).Select(x => x.PayMethodId).ToList();
            return paymentMethodIds;
        }

        public bool DeletePayMethodInPayTerm(int payTermId, int methodId)
        {
            var doc = GetPayMethodInPayTerm(payTermId, methodId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<PayMethodsInPayTerm>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public List<PaymentTermViewModel> GetPaymentTermsWithoutOnAccountType()
        {
            var mapper = new PaymentTermToPaymentTermViewModelMapper();
            var paymentTerms =
                _dbContext.Set<PaymentTerm>()
                    .Where(x => x.CompanyId == LoggedInUser.CompanyId && x.Active && x.TermType != TermType.OnAccount)
                    .ToList();
            return paymentTerms.Select(x => mapper.Map(x)).ToList();
        }

        public PayMethodsInPayTerm GetPayMethodInPayTerm(int termId, int methodId)
        {
            var payMethodsInPayTerm = _dbContext.Set<PayMethodsInPayTerm>().FirstOrDefault(d => d.PayTermId == termId && d.PayMethodId == methodId);
            return payMethodsInPayTerm;
        }

        public List<PaymentMethodViewModel> GetPayMethodsInPayTerm(int id)
        {
            var paymentMethodMapper = new PaymentMethodToPaymentMethodViewModelMapper();
            var paymentMethodIds = GetPayMethodIdsByTermId(id);
            var paymentMethods =
                paymentMethodIds.Select(
                        paymentMethodId => _dbContext.Set<PaymentMethod>().SingleOrDefault(x => x.Id == paymentMethodId))
                    .ToList();
            return paymentMethods.Select(x => paymentMethodMapper.Map(x)).ToList();
        }

        public void SaveAllPayMethodsInPayTerm(List<PayMethodsInPayTerm> payMethodsInPayTermList)
        {
            foreach (var payMethodsInPayTerm in payMethodsInPayTermList)
            {
                payMethodsInPayTerm.CompanyId = LoggedInUser.CompanyId;
                payMethodsInPayTerm.Active = true;
            }
            _dbContext.Set<PayMethodsInPayTerm>().AddRange(payMethodsInPayTermList);
            _dbContext.SaveChanges();
        }

       public PaymentTerm CheckIfDeletedPaymentTermWithSameCodeExists(string code)
        {
            var paymentTerm =
                _dbContext.Set<PaymentTerm>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.TermCode == code && (x.Active ||
                             x.Active == false));
            return paymentTerm;
        }

        public PaymentTerm CheckIfDeletedPaymentTermWithSameNameExists(string name)
        {
            var paymentTerm =
                _dbContext.Set<PaymentTerm>()
                    .FirstOrDefault(
                        x => x.CompanyId == LoggedInUser.CompanyId &&
                             x.TermName == name && (x.Active ||
                                                    x.Active == false));
            return paymentTerm;
        }

        public IQueryable<PaymentTerm> SearchPaymentTerms(bool active, string searchText)
        {
            var paymentTerms = _dbContext.Set<PaymentTerm>().Where(x => x.CompanyId == LoggedInUser.CompanyId);
            if (active)
                paymentTerms = paymentTerms.Where(x => x.Active);
            var query = string.IsNullOrEmpty(searchText) || searchText.Contains("null")
                ? paymentTerms
                : paymentTerms.Where(s => s.TermCode.ToUpper().Contains(searchText.ToUpper()));
            return query;
        }

        public bool DeleteRange(List<int?> paymentTermsId)
        {

            var paymentTermList = paymentTermsId.Select(paymentTermId => _dbContext.Set<PaymentTerm>().FirstOrDefault(x => x.Id == paymentTermId && x.CompanyId == LoggedInUser.CompanyId)).ToList();
            paymentTermList.ForEach(x => x.Active = false);
            _dbContext.Set<PaymentTerm>().UpdateRange(paymentTermList);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
