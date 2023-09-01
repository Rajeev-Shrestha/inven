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
    public class MediaTypesQueryProcessor : QueryBase<MediaTypes>, IMediaTypesQueryProcessor
    {
        public MediaTypesQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }
        public MediaTypes Update(MediaTypes mediaTypes)
        {
            var original = GetValidMediaTypes(mediaTypes.Id);
            ValidateAuthorization(mediaTypes);
            CheckVersionMismatch(mediaTypes, original);

            original.Value = mediaTypes.Value;
            original.Active = mediaTypes.Active;
            original.CompanyId = mediaTypes.CompanyId;

            _dbContext.Set<MediaTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public virtual MediaTypes GetValidMediaTypes(int mediaTypesId)
        {
            var mediaTypes = _dbContext.Set<MediaTypes>().FirstOrDefault(sc => sc.Id == mediaTypesId);
            if (mediaTypes == null)
            {
                throw new RootObjectNotFoundException("Media Types not found");
            }
            return mediaTypes;
        }
        public MediaTypes GetMediaTypes(int mediaTypesId)
        {
            var mediaTypes = _dbContext.Set<MediaTypes>().FirstOrDefault(d => d.Id == mediaTypesId);
            return mediaTypes;
        }
        public void SaveAllMediaTypes(List<MediaTypes> mediaTypes)
        {
            _dbContext.Set<MediaTypes>().AddRange(mediaTypes);
            _dbContext.SaveChanges();
        }
        public MediaTypes Save(MediaTypes mediaTypes)
        {
            mediaTypes.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<MediaTypes>().Add(mediaTypes);
            _dbContext.SaveChanges();
            return mediaTypes;
        }
        public int SaveAll(List<MediaTypes> mediaTypes)
        {
            _dbContext.Set<MediaTypes>().AddRange(mediaTypes);
            return _dbContext.SaveChanges();
        }
        public MediaTypes ActivateMediaTypes(int id)
        {
            var original = GetValidMediaTypes(id);
            ValidateAuthorization(original);

            original.Active = true;
            _dbContext.Set<MediaTypes>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
        public MediaTypeViewModel GetMediaTypesViewModel(int id)
        {
            var mediaTypes = _dbContext.Set<MediaTypes>().Single(s => s.Id == id);
            var mapper = new MediaTypeToMediaTypeViewModelMapper();
            return mapper.Map(mediaTypes);
        }
        public bool Delete(int mediaTypesId)
        {
            var doc = GetMediaTypes(mediaTypesId);
            ValidateAuthorization(doc);
            var result = 0;
            if (doc == null) return result > 0;
            doc.Active = false;
            _dbContext.Set<MediaTypes>().Update(doc);
            result = _dbContext.SaveChanges();
            return result > 0;
        }
        public bool Exists(Expression<Func<MediaTypes, bool>> @where)
        {
            return _dbContext.Set<MediaTypes>().Any(@where);
        }
        public MediaTypes[] GetMediaTypes(Expression<Func<MediaTypes, bool>> @where = null)
        {

            var query = _dbContext.Set<MediaTypes>().Where(FilterByActiveTrueAndCompany);
            query = @where == null ? query : query.Where(@where);

            var enumerable = query.ToArray();
            return enumerable;
        }
        public IQueryable<MediaTypes> GetActiveMediaTypes()
        {
            return _dbContext.Set<MediaTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active);
        }
        public IQueryable<MediaTypes> GetDeletedMediaTypes()
        {
            return _dbContext.Set<MediaTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId && f.Active == false);
        }
        public IQueryable<MediaTypes> GetAllMediaTypes()
        {
            var result = _dbContext.Set<MediaTypes>().Where(f => f.CompanyId == LoggedInUser.CompanyId);
            return result;
        }
    }
}
