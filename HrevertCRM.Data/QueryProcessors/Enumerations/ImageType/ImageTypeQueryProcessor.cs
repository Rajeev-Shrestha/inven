using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Data.Mapper.Enumerations;
using HrevertCRM.Data.QueryProcessors.Enumerations;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public class ImageTypeQueryProcessor:QueryBase<ImageTypes>,IImageTypeQueryProcessor
    {
        public ImageTypeQueryProcessor(IUserSession userSession,IDbContext dbContext):base(userSession, dbContext)
        {

        }
        public ImageTypes Update(ImageTypes ImageTypes)
        {
            var original = GetValidImageTypes(ImageTypes.Id);
            ValidateAuthorization(ImageTypes);
            CheckVersionMismatch(ImageTypes, original);

            original.Value = ImageTypes.Value;
            original.Active = ImageTypes.Active;
            original.CompanyId = ImageTypes.CompanyId;

            _dbContext.Set<ImageTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual ImageTypes GetValidImageTypes(int ImageTypesId)
        {
            var ImageTypes = _dbContext.Set<ImageTypes>().FirstOrDefault(sc => sc.Id == ImageTypesId);
            if (ImageTypes == null)
            {
                throw new RootObjectNotFoundException("ImageTypes not found");
            }
            return ImageTypes;
        }
        public ImageTypes GetImageTypes(int ImageTypesId)
        {
            var ImageTypes = _dbContext.Set<ImageTypes>().FirstOrDefault(d => d.Id == ImageTypesId);
            return ImageTypes;
        }
        public void SaveAllImageTypes(List<ImageTypes> ImageTypes)
        {
            _dbContext.Set<ImageTypes>().AddRange(ImageTypes);
            _dbContext.SaveChanges();
        }
        public ImageTypes Save(ImageTypes ImageTypes)
        {
            ImageTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<ImageTypes>().Add(ImageTypes);
            _dbContext.SaveChanges();
            return ImageTypes;
        }
        public int SaveAll(List<ImageTypes> ImageTypes)
        {
            _dbContext.Set<ImageTypes>().AddRange(ImageTypes);
            return _dbContext.SaveChanges();
        }
        public ImageTypes ActivateImageTypes(int id)
        {
            var original = GetValidImageTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<ImageTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public ImageTypeViewModel GetImageTypesViewModel(int id)
        {
            var ImageTypes = _dbContext.Set<ImageTypes>().Single(s => s.Id == id);
            var mapper = new ImageTypesToImageTypeViewModelMapper();
            return mapper.Map(ImageTypes);
        }
        public bool Delete(int ImageTypesId)
        {
            var doc = GetImageTypes(ImageTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<ImageTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<ImageTypes, bool>> @where)
        {
            return _dbContext.Set<ImageTypes>().Any(@where);
        }
        public ImageTypes[] GetImageTypes(Expression<Func<ImageTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<ImageTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<ImageTypes> GetActiveImageTypes()
        {
            return _dbContext.Set<ImageTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<ImageTypes> GetDeletedImageTypes()
        {
            return _dbContext.Set<ImageTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<ImageTypes> GetAllImageTypes()
        {
            var result = _dbContext.Set<ImageTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }

    }
}
