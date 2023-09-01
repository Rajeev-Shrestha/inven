using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;


namespace HrevertCRM.Data.QueryProcessors
{
    public interface IPaymentTermQueryProcessor
    {
        PaymentTerm Update(PaymentTerm paymentTerm);
        PaymentTerm GetPaymentTerm(int paymentTermId);
        void SaveAllPaymentTerm(List<PaymentTerm> paymentTerms);
        bool Delete(int paymentTermId);
        bool Exists(Expression<Func<PaymentTerm, bool>> @where);
        QueryResult<PaymentTerm> GetPaymentTerms(PagedDataRequest requestInfo,
            Expression<Func<PaymentTerm, bool>> @where = null);
        PaymentTerm[] GetPaymentTerms(Expression<Func<PaymentTerm, bool>> @where = null);
        PaymentTerm Save(PaymentTerm paymentTerm);
        int SaveAll(List<PaymentTerm> paymentTerms);
        PaymentTerm ActivatePaymentTerm(int id);
        List<PaymentTerm> GetOnAccountTerms();
        PayMethodsInPayTerm SavePayMethodInPayTerms(PayMethodsInPayTerm payMethodsInPayTerm);
        List<PaymentMethodViewModel> GetPayMethodsInPayTerm(int id);
        void SaveAllPayMethodsInPayTerm(List<PayMethodsInPayTerm> payMethodsInPayTermList);
        List<int> GetPayMethodIdsByTermId(int id);
        bool DeletePayMethodInPayTerm(int payTermId, int methodId);
        List<PaymentTermViewModel> GetPaymentTermsWithoutOnAccountType();
        PaymentTerm CheckIfDeletedPaymentTermWithSameCodeExists(string code);
        PaymentTerm CheckIfDeletedPaymentTermWithSameNameExists(string name);
        IQueryable<PaymentTerm> SearchPaymentTerms(bool active, string searchText);
        bool DeleteRange(List<int?> peymentTermsId);
    }
}
