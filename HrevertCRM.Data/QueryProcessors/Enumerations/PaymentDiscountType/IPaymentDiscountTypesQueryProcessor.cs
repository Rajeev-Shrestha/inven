using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IPaymentDiscountTypesQueryProcessor
    {
        PaymentDiscountTypes Update(PaymentDiscountTypes paymentDiscountTypes);
        PaymentDiscountTypes GetValidPaymentDiscountTypes(int paymentDiscountTypesId);
        PaymentDiscountTypes GetPaymentDiscountTypes(int paymentDiscountTypesId);
        void SaveAllPaymentDiscountTypes(List<PaymentDiscountTypes> paymentDiscountTypes);
        PaymentDiscountTypes Save(PaymentDiscountTypes paymentDiscountTypes);
        int SaveAll(List<PaymentDiscountTypes> paymentDiscountTypes);
        PaymentDiscountTypes ActivatePaymentDiscountTypes(int id);
        PaymentDiscountTypeViewModel GetPaymentDiscountTypesViewModel(int id);
        bool Delete(int paymentDiscountTypesId);
        bool Exists(Expression<Func<PaymentDiscountTypes, bool>> @where);
        PaymentDiscountTypes[] GetPaymentDiscountTypes(Expression<Func<PaymentDiscountTypes, bool>> @where = null);
        IQueryable<PaymentDiscountTypes> GetActivePaymentDiscountTypes();
        IQueryable<PaymentDiscountTypes> GetDeletedPaymentDiscountTypes();
        IQueryable<PaymentDiscountTypes> GetAllPaymentDiscountTypes();
    }
}