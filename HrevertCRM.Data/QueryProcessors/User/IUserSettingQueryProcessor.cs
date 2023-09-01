using System.Collections.Generic;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IUserSettingQueryProcessor
    {
        UserSetting Update(UserSetting userSetting);
        UserSetting GetValidUserSetting(int userSettingId);
        UserSetting GetUserSetting(int userSettingId);
        int SaveAll(List<UserSetting> companies);
        UserSetting Save(UserSetting userSetting);
        int GetPageSize(EntityId entityId);
        int ForPaging(PagedDataRequest requestInfo, EntityId id);
    }
}