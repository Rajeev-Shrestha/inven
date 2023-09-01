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
    public class PaymentDiscountTypesQueryProcessor : QueryBase<PaymentDiscountTypes>, IPaymentDiscountTypesQueryProcessor
    {
        public PaymentDiscountTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public PaymentDiscountTypes Update(PaymentDiscountTypes paymentDiscountTypes)
        {
            var original = GetValidPaymentDiscountTypes(paymentDiscountTypes.Id);
            ValidateAuthorization(paymentDiscountTypes);
            CheckVersionMismatch(paymentDiscountTypes, original);

            original.Value = paymentDiscountTypes.Value;
            original.Active = paymentDiscountTypes.Active;
            original.CompanyId = paymentDiscountTypes.CompanyId;

            _dbContext.Set<PaymentDiscountTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual PaymentDiscountTypes GetValidPaymentDiscountTypes(int paymentDiscountTypesId)
        {
            var paymentDiscountTypes = _dbContext.Set<PaymentDiscountTypes>().FirstOrDefault(sc => sc.Id == paymentDiscountTypesId);
            if (paymentDiscountTypes == null)
            {
                throw new RootObjectNotFoundException("Payment Discount Types not found");
            }
            return paymentDiscountTypes;
        }
        public PaymentDiscountTypes GetPaymentDiscountTypes(int paymentDiscountTypesId)
        {
            var paymentDiscountTypes = _dbContext.Set<PaymentDiscountTypes>().FirstOrDefault(d => d.Id == paymentDiscountTypesId);
            return paymentDiscountTypes;
        }
        public void SaveAllPaymentDiscountTypes(List<PaymentDiscountTypes> paymentDiscountTypes)
        {
            _dbContext.Set<PaymentDiscountTypes>().AddRange(paymentDiscountTypes);
            _dbContext.SaveChanges();
        }
        public PaymentDiscountTypes Save(PaymentDiscountTypes paymentDiscountTypes)
        {
            paymentDiscountTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<PaymentDiscountTypes>().Add(paymentDiscountTypes);
            _dbContext.SaveChanges();
            return paymentDiscountTypes;
        }
        public int SaveAll(List<PaymentDiscountTypes> paymentDiscountTypes)
        {
            _dbContext.Set<PaymentDiscountTypes>().AddRange(paymentDiscountTypes);
            return _dbContext.SaveChanges();
        }
        public PaymentDiscountTypes ActivatePaymentDiscountTypes(int id)
        {
            var original = GetValidPaymentDiscountTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<PaymentDiscountTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public PaymentDiscountTypeViewModel GetPaymentDiscountTypesViewModel(int id)
        {
            var paymentDiscountTypes = _dbContext.Set<PaymentDiscountTypes>().Single(s => s.Id == id);
            var mapper = new PaymentDiscountTypeToPaymentDiscountTypeViewModelMapper();
            return mapper.Map(paymentDiscountTypes);
        }
        public bool Delete(int paymentDiscountTypesId)
        {
            var doc = GetPaymentDiscountTypes(paymentDiscountTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<PaymentDiscountTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<PaymentDiscountTypes, bool>> @where)
        {
            return _dbContext.Set<PaymentDiscountTypes>().Any(@where);
        }
        public PaymentDiscountTypes[] GetPaymentDiscountTypes(Expression<Func<PaymentDiscountTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<PaymentDiscountTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<PaymentDiscountTypes> GetActivePaymentDiscountTypes()
        {
            return _dbContext.Set<PaymentDiscountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<PaymentDiscountTypes> GetDeletedPaymentDiscountTypes()
        {
            return _dbContext.Set<PaymentDiscountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<PaymentDiscountTypes> GetAllPaymentDiscountTypes()
        {
            var result = _dbContext.Set<PaymentDiscountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
