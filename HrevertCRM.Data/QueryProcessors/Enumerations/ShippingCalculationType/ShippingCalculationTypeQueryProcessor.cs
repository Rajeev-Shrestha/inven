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
   
        public class ShippingCalculationTypeQueryProcessor : QueryBase<ShippingCalculationTypes>,IShippingCalculationTypeQueryProcessor
        {
            public ShippingCalculationTypeQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
            {
            }
            public ShippingCalculationTypes Update(ShippingCalculationTypes shippingCalculationType)
            {
                var original = GetValidShippingCalculationType(shippingCalculationType.Id);
                ValidateAuthorization(shippingCalculationType);
                CheckVersionMismatch(shippingCalculationType, original);

                original.Value = shippingCalculationType.Value;
                original.Active = shippingCalculationType.Active;
                original.CompanyId = shippingCalculationType.CompanyId;

                _dbContext.Set<ShippingCalculationTypes>().Update(original);
                _dbContext.SaveChanges();
                return original;
            }
            public virtual ShippingCalculationTypes GetValidShippingCalculationType(int shippingCalculationTypeId)
            {
                var shippingCalculationType = _dbContext.Set<ShippingCalculationTypes>().FirstOrDefault(sc => sc.Id == shippingCalculationTypeId);
                if (shippingCalculationType == null)
                {
                    throw new RootObjectNotFoundException("Shipping Calculation Type not found");
                }
                return shippingCalculationType;
            }
            public ShippingCalculationTypes GetShippingCalculationType(int shippingCalculationTypeId)
            {
                var shippingCalculationType = _dbContext.Set<ShippingCalculationTypes>().FirstOrDefault(d => d.Id == shippingCalculationTypeId);
                return shippingCalculationType;
            }
            public void SaveAllShippingCalculationType(List<ShippingCalculationTypes> shippingCalculationType)
            {
                _dbContext.Set<ShippingCalculationTypes>().AddRange(shippingCalculationType);
                _dbContext.SaveChanges();
            }
            public ShippingCalculationTypes Save(ShippingCalculationTypes shippingCalculationType)
            {
                shippingCalculationType.CompanyId = LoggedInUser.CompanyId;
                _dbContext.Set<ShippingCalculationTypes>().Add(shippingCalculationType);
                _dbContext.SaveChanges();
                return shippingCalculationType;
            }
            public int SaveAll(List<ShippingCalculationTypes> shippingCalculationType)
            {
                _dbContext.Set<ShippingCalculationTypes>().AddRange(shippingCalculationType);
                return _dbContext.SaveChanges();
            }
            public ShippingCalculationTypes ActivateShippingCalculationType(int id)
            {
                var original = GetValidShippingCalculationType(id);
                ValidateAuthorization(original);

                original.Active = true;
                _dbContext.Set<ShippingCalculationTypes>().Update(original);
                _dbContext.SaveChanges();
                return original;
            }
            public ShippingCalculationTypeViewModel GetShippingCalculationTypeViewModel(int id)
            {
                var ShippingCalculationType = _dbContext.Set<ShippingCalculationTypes>().Single(s => s.Id == id);
                var mapper = new ShippingCalculationTypeToShippingCalculationTypeViewModelMapper();
                return mapper.Map(ShippingCalculationType);
            }
            public bool Delete(int shippingCalculationTypeId)
            {
                var doc = GetShippingCalculationType(shippingCalculationTypeId);
                ValidateAuthorization(doc);
                var result = 0;
                if (doc == null) return result > 0;
                doc.Active = false;
                _dbContext.Set<ShippingCalculationTypes>().Update(doc);
                result = _dbContext.SaveChanges();
                return result > 0;
            }
            public bool Exists(Expression<Func<ShippingCalculationTypes, bool>> @where)
            {
                return _dbContext.Set<ShippingCalculationTypes>().Any(@where);
            }
            public ShippingCalculationTypes[] GetShippingCalculationType(Expression<Func<ShippingCalculationTypes, bool>> @where = null)
            {

                var query = _dbContext.Set<ShippingCalculationTypes>().Where(FilterByActiveTrueAndCompany);
                query = @where == null ? query : query.Where(@where);

                var enumerable = query.ToArray();
                return enumerable;
            }
            public IQueryable<ShippingCalculationTypes> GetActiveShippingCalculationType()
            {
                return _dbContext.Set<ShippingCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
            }
            public IQueryable<ShippingCalculationTypes> GetDeletedShippingCalculationType()
            {
                return _dbContext.Set<ShippingCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
            }
            public IQueryable<ShippingCalculationTypes> GetAllShippingCalculationType()
            {
                var result = _dbContext.Set<ShippingCalculationTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
                return result;
            }
        }
    }

