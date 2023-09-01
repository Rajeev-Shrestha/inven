using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IPaymentMethodQueryProcessor
    {
        PaymentMethod Update(PaymentMethod paymentMethod);
        PaymentMethod GetValidPaymentMethod(int paymentMethodId);
        PaymentMethod GetPaymentMethod(int paymentMethodId);
        void SaveAllPaymentMethod(List<PaymentMethod> paymentMethods);
        PaymentMethod Save(PaymentMethod paymentMethod);
        int SaveAll(List<PaymentMethod> paymentMethods);
        bool Delete(int paymentMethodId);
        bool Exists(Expression<Func<PaymentMethod, bool>> @where);
        QueryResult<PaymentMethod> GetPaymentMethods(PagedDataRequest requestInfo,
            Expression<Func<PaymentMethod, bool>> @where = null);
        PaymentMethod[] GetPaymentMethods(Expression<Func<PaymentMethod, bool>> @where = null);
        PaymentMethod ActivatePaymentMethod(int id);
        PaymentMethod CheckIfDeletedPaymentMethodWithSameCodeExists(string code);
        PaymentMethod CheckIfDeletedPaymentMethodWithSameNameExists(string name);
        IQueryable<PaymentMethod> SearchPaymentMethods(bool active, string searchText);
        bool DeleteRange(List<int?> paymentMethodsId);
    }
}