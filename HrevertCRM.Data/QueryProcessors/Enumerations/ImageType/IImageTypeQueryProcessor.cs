using HrevertCRM.Data.ViewModels.Enumerations;
using HrevertCRM.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HrevertCRM.Data.QueryProcessors.Enumerations
{
    public interface IImageTypeQueryProcessor
    {
        ImageTypes Update(ImageTypes imageType);
        ImageTypes GetValidImageTypes(int imageTypeId);
        ImageTypes GetImageTypes(int imageTypesId);
        void SaveAllImageTypes(List<ImageTypes> umageTypes);
        ImageTypes Save(ImageTypes imageType);
        int SaveAll(List<ImageTypes> imageTypes);
        ImageTypes ActivateImageTypes(int id);
        ImageTypeViewModel GetImageTypesViewModel(int id);
        bool Delete(int imageTypeId);
        bool Exists(Expression<Func<ImageTypes, bool>> @where);
        ImageTypes[] GetImageTypes(Expression<Func<ImageTypes, bool>> @where = null);
        IQueryable<ImageTypes> GetActiveImageTypes();
        IQueryable<ImageTypes> GetDeletedImageTypes();
        IQueryable<ImageTypes> GetAllImageTypes();
    }
}
