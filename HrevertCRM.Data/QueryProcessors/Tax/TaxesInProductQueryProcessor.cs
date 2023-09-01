using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hrevert.Common.Security;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;

namespace HrevertCRM.Data
{
    public class TaxesInProductQueryProcessor : QueryBase<TaxesInProduct>, ITaxesInProductQueryProcessor
    {
        public TaxesInProductQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public int SaveAll(List<TaxesInProduct> taxesInProducts)
        {
            _dbContext.Set<TaxesInProduct>().AddRange(taxesInProducts);
            return _dbContext.SaveChanges();
        }

        public int SaveAllTaxesInProducts(List<TaxesInProduct> taxesInProducts)
        {
            taxesInProducts.ForEach(x => x.CompanyId = LoggedInUser.CompanyId);
            _dbContext.Set<TaxesInProduct>().AddRange(taxesInProducts);
            return _dbContext.SaveChanges();
        }

        public bool Delete(int productId, int taxId)
        {
            var doc = GetTaxesInProduct(productId, taxId);
            var result = 0;
            if (doc == null) return result > 0;
            _dbContext.Set<TaxesInProduct>().Remove(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }

        public TaxesInProduct GetTaxesInProduct(int productId, int taxId)
        {
            var taxesInProduct =
                _dbContext.Set<TaxesInProduct>().FirstOrDefault(x => x.ProductId == productId && x.TaxId == taxId && x.CompanyId == LoggedInUser.CompanyId && x.Active);
            return taxesInProduct;
        }

        public List<int> GetExistingTaxesOfProduct(int productId)
        {
            var existingTaxes = _dbContext.Set<TaxesInProduct>().Where(p => p.ProductId == productId && p.CompanyId == LoggedInUser.CompanyId && p.Active).Select(p => p.TaxId).ToList();
            return existingTaxes;
        }

        public bool Exists(Expression<Func<TaxesInProduct, bool>> @where)
        {
            var check = _dbContext.Set<TaxesInProduct>().Any(@where);
            return check;
        }

        public TaxesInProduct Save(TaxesInProduct taxesInProduct)
        {
            _dbContext.Set<TaxesInProduct>().Add(taxesInProduct);
            _dbContext.SaveChanges();
            return taxesInProduct;
        }

        
    }
}
