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
    public class DiscountTypesQueryProcessor : QueryBase<DiscountTypes>, IDiscountTypesQueryProcessor
    {
        public DiscountTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public DiscountTypes Update(DiscountTypes discountTypes)
        {
            var original = GetValidDiscountTypes(discountTypes.Id);
            ValidateAuthorization(discountTypes);
            CheckVersionMismatch(discountTypes, original);

            original.Value = discountTypes.Value;
            original.Active = discountTypes.Active;
            original.CompanyId = discountTypes.CompanyId;

            _dbContext.Set<DiscountTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual DiscountTypes GetValidDiscountTypes(int discountTypesId)
        {
            var discountTypes = _dbContext.Set<DiscountTypes>().FirstOrDefault(sc => sc.Id == discountTypesId);
            if (discountTypes == null)
            {
                throw new RootObjectNotFoundException("Discount Types not found");
            }
            return discountTypes;
        }
        public DiscountTypes GetDiscountTypes(int discountTypesId)
        {
            var discountTypes = _dbContext.Set<DiscountTypes>().FirstOrDefault(d => d.Id == discountTypesId);
            return discountTypes;
        }
        public void SaveAllDiscountTypes(List<DiscountTypes> discountTypes)
        {
            _dbContext.Set<DiscountTypes>().AddRange(discountTypes);
            _dbContext.SaveChanges();
        }
        public DiscountTypes Save(DiscountTypes discountTypes)
        {
            discountTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<DiscountTypes>().Add(discountTypes);
            _dbContext.SaveChanges();
            return discountTypes;
        }
        public int SaveAll(List<DiscountTypes> discountTypes)
        {
            _dbContext.Set<DiscountTypes>().AddRange(discountTypes);
            return _dbContext.SaveChanges();
        }
        public DiscountTypes ActivateDiscountTypes(int id)
        {
            var original = GetValidDiscountTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DiscountTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public DiscountTypeViewModel GetDiscountTypesViewModel(int id)
        {
            var discountTypes = _dbContext.Set<DiscountTypes>().Single(s => s.Id == id);
            var mapper = new DiscountTypeToDiscountTypeViewModelMapper();
            return mapper.Map(discountTypes);
        }
        public bool Delete(int discountTypesId)
        {
            var doc = GetDiscountTypes(discountTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DiscountTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<DiscountTypes, bool>> @where)
        {
            return _dbContext.Set<DiscountTypes>().Any(@where);
        }
        public DiscountTypes[] GetDiscountTypes(Expression<Func<DiscountTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<DiscountTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<DiscountTypes> GetActiveDiscountTypes()
        {
            return _dbContext.Set<DiscountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<DiscountTypes> GetDeletedDiscountTypes()
        {
            return _dbContext.Set<DiscountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<DiscountTypes> GetAllDiscountTypes()
        {
            var result = _dbContext.Set<DiscountTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
