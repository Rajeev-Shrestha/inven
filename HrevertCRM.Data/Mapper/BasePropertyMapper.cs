using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.Mapper
{
    public static class BasePropertyMapper
    {

        public static BaseEntity Map(BaseEntity fromEntity, BaseEntity toEntity)
        {
            toEntity.CompanyId = fromEntity.CompanyId;
            toEntity.DateCreated = fromEntity.DateCreated;
            toEntity.DateModified = fromEntity.DateModified;
            toEntity.Active = fromEntity.Active;
            toEntity.LastUpdatedBy = fromEntity.LastUpdatedBy;

            return toEntity;

        }
    }
}
