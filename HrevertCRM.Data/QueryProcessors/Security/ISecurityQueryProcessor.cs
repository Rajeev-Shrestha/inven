using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISecurityQueryProcessor
    {
        bool VerifyUserHasRight(long securityCode);

        Security Save(Security securtiy);
        bool Exists(Expression<Func<Security, bool>> @where);
        PagedDataInquiryResponse<SecurityViewModel> GetSecurities(PagedDataRequest requestInfo, Expression<Func<Security, bool>> @where = null);
        Security Update(Security security);
        bool Delete(int securityId);
        Security GetSecurity(int securityId);
        IQueryable<Security> GetActiveSecurities();
        IQueryable<Security> GetDeletedSecurities();
        int SaveAll(List<Security> securities);

        PagedDataInquiryResponse<SecurityViewModel> SearchSecurities(PagedDataRequest requestInfo, 
            Expression<Func<Security, bool>> @where = null);

        List<int?> GetGroupsWithAuthorityToAssignRight();
    }
}
