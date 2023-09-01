using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IProductPriceRuleQueryProcessor
    {
        ProductPriceRule Update(ProductPriceRule productPriceRule);
        ProductPriceRule GetProductPriceRule(int productPriceRuleId);
        ProductPriceRuleViewModel GetProductPriceRuleViewModel(int productPriceRuleId);
        void SaveAllProductPriceRule(List<ProductPriceRule> productPriceRules);
        bool Delete(int productPriceRuleId);
        bool Exists(Expression<Func<ProductPriceRule, bool>> where);
        PagedDataInquiryResponse<ProductPriceRuleViewModel> GetActiveProductPriceRules(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> @where = null);
        PagedDataInquiryResponse<ProductPriceRuleViewModel> GetDeletedProductPriceRules(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> @where = null);
        PagedDataInquiryResponse<ProductPriceRuleViewModel> GetAllProductPriceRules(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> @where = null);
        PagedDataInquiryResponse<ProductPriceRuleViewModel> SearchActive(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> @where = null);
        PagedDataInquiryResponse<ProductPriceRuleViewModel> SearchAll(PagedDataRequest requestInfo, Expression<Func<ProductPriceRule, bool>> @where = null);
        ProductPriceRule ActivateProductPriceRule(int id);
        ProductPriceRule[] GetProductPriceRules(Expression<Func<ProductPriceRule, bool>> where = null);
        int GetCompanyIdByProductPriceRuleId(int productPriceRuleId);
        ApplicationUser ActiveUser { get; }
        ProductPriceRule Save(ProductPriceRule productPriceRule);
    }
}