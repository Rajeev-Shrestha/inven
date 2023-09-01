using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class DiscountCalculationTypeQueryProcessor:QueryBase<DiscountCalculationTypes>,IDiscountCalculationTypeQueryProcessor
    {
        public DiscountCalculationTypeQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public DiscountCalculationTypes Update(DiscountCalculationTypes discountCalculationType)
        {
            var original = GetValidDiscountCalculationType(discountCalculationType.Id);
            ValidateAuthorization(discountCalculationType);
            CheckVersionMismatch(discountCalculationType, original);

            original.Value = discountCalculationType.Value;
            original.Active = discountCalculationType.Active;
            original.CompanyId = discountCalculationType.CompanyId;

            _dbContext.Set<DiscountCalculationTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual DiscountCalculationTypes GetValidDiscountCalculationType(int DiscountCalculationTypeId)
        {
            var DiscountCalculationType = _dbContext.Set<DiscountCalculationTypes>().FirstOrDefault(sc => sc.Id == DiscountCalculationTypeId);
            if (DiscountCalculationType == null)
            {
                throw new RootObjectNotFoundException("Discount Calculation Types not found");
            }
            return DiscountCalculationType;
        }
        public DiscountCalculationTypes GetDiscountCalculationType(int discountCalculationTypeId)
        {
            var discountCalculationType = _dbContext.Set<DiscountCalculationTypes>().FirstOrDefault(d => d.Id == discountCalculationTypeId);
            return discountCalculationType;
        }
        public void SaveAllDiscountCalculationType(List<DiscountCalculationTypes> discountCalculationType)
        {
            _dbContext.Set<DiscountCalculationTypes>().AddRange(discountCalculationType);
            _dbContext.SaveChanges();
        }
        public DiscountCalculationTypes Save(DiscountCalculationTypes discountCalculationType)
        {
            discountCalculationType.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<DiscountCalculationTypes>().Add(discountCalculationType);
            _dbContext.SaveChanges();
            return discountCalculationType;
        }
        public int SaveAll(List<DiscountCalculationTypes> discountCalculationType)
        {
            _dbContext.Set<DiscountCalculationTypes>().AddRange(discountCalculationType);
            return _dbContext.SaveChanges();
        }
        public DiscountCalculationTypes ActivateDiscountCalculationType(int id)
        {
            var original = GetValidDiscountCalculationType(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<DiscountCalculationTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public DiscountCalculationTypeViewModel GetDiscountCalculationTypeViewModel(int id)
        {
            var DiscountCalculationType = _dbContext.Set<DiscountCalculationTypes>().Single(s => s.Id == id);
            var mapper = new DiscountCalculationTypeToDiscountCalculationTypeViewModelMapper();
            return mapper.Map(DiscountCalculationType);
        }
        public bool Delete(int discountCalculationTypeId)
        {
            var doc = GetDiscountCalculationType(discountCalculationTypeId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<DiscountCalculationTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<DiscountCalculationTypes, bool>> @where)
        {
            return _dbContext.Set<DiscountCalculationTypes>().Any(@where);
        }
        public DiscountCalculationTypes[] GetDiscountCalculationType(Expression<Func<DiscountCalculationTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<DiscountCalculationTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<DiscountCalculationTypes> GetActiveDiscountCalculationType()
        {
            return _dbContext.Set<DiscountCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<DiscountCalculationTypes> GetDeletedDiscountCalculationType()
        {
            return _dbContext.Set<DiscountCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<DiscountCalculationTypes> GetAllDiscountCalculationType()
        {
            var result = _dbContext.Set<DiscountCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
