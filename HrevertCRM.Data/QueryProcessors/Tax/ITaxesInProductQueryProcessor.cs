using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Entities;

namespace HrevertCRM.Data
{
    public interface ITaxesInProductQueryProcessor
    {
        int SaveAll(List<TaxesInProduct> taxesInProducts);
        int SaveAllTaxesInProducts(List<TaxesInProduct> taxesInProducts);
        bool Delete(int productId, int taxId);
        TaxesInProduct GetTaxesInProduct(int productId, int taxId);
        List<int> GetExistingTaxesOfProduct(int productId);
        bool Exists(Expression<Func<TaxesInProduct, bool>> @where);
        TaxesInProduct Save(TaxesInProduct taxesInProduct);
    }
}
