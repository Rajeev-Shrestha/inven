using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IMediaTypesQueryProcessor
    {
        MediaTypes Update(MediaTypes mediaTypes);
        MediaTypes GetValidMediaTypes(int mediaTypesId);
        MediaTypes GetMediaTypes(int mediaTypesId);
        void SaveAllMediaTypes(List<MediaTypes> mediaTypes);
        MediaTypes Save(MediaTypes mediaTypes);
        int SaveAll(List<MediaTypes> mediaTypes);
        MediaTypes ActivateMediaTypes(int id);
        MediaTypeViewModel GetMediaTypesViewModel(int id);
        bool Delete(int mediaTypesId);
        bool Exists(Expression<Func<MediaTypes, bool>> @where);
        MediaTypes[] GetMediaTypes(Expression<Func<MediaTypes, bool>> @where = null);
        IQueryable<MediaTypes> GetActiveMediaTypes();
        IQueryable<MediaTypes> GetDeletedMediaTypes();
        IQueryable<MediaTypes> GetAllMediaTypes();
    }
}