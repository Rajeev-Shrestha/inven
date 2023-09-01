using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IBranchQueryProcessor
    {
        Branch UpdateBranch(Branch branch);
        Branch GetBranch(int branchId);
        void SaveAllBranch(List<Branch> branches);
        bool DeleteBranch(int branchId);
        bool BranchExists(Func<Branch, bool> @where);
        QueryResult<Branch> GetBranches(PagedDataRequest requestInfo, Func<Branch, bool> @where = null);
        Company[] GetBranches(Func<Company, bool> @where = null);
        IQueryable<Branch> GetAllBranches();
        Branch Save(Branch branch);
    }
}
