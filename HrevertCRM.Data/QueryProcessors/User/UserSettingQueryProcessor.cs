using System.Collections.Generic;
using System.Linq;
using Hrevert.Common.Constants.User;
using Hrevert.Common.Security;
using HrevertCRM.Data.Common;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public class UserSettingQueryProcessor : QueryBase<UserSetting>, IUserSettingQueryProcessor
    {
        public UserSettingQueryProcessor(IUserSession userSession, IDbContext dbContext) : base(userSession, dbContext)
        {
        }

        public UserSetting Update(UserSetting userSetting)
        {
            var original = GetValidUserSetting(userSetting.Id);

            original.PageSize = userSetting.PageSize;

            _dbContext.Set<UserSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }

        public virtual UserSetting GetValidUserSetting(int userSettingId)
        {
            var userSetting = _dbContext.Set<UserSetting>().FirstOrDefault(sc => sc.Id == userSettingId);
            if (userSetting == null)
            {
                throw new RootObjectNotFoundException(UserConstants.UserSettingQueryProcessorConstants.UserSettingNotFound);
            }
            return userSetting;
        }


        public UserSetting GetUserSetting(int userSettingId)
        {
            var userSetting = _dbContext.Set<UserSetting>().FirstOrDefault(d => d.Id == userSettingId);
            return userSetting;
        }

        public int GetPageSize(EntityId entityId)
        {
            var userSetting = _dbContext.Set<UserSetting>().FirstOrDefault(d => d.EntityId == entityId && d.UserId == LoggedInUser.Id);

            if (userSetting == null || userSetting.PageSize == 0)
            {
                return 0;
            }
            else
            {
                return userSetting.PageSize;
            }
        }

        public UserSetting Save(UserSetting userSetting)
        {
            var setting = _dbContext
                    .Set<UserSetting>()
                    .FirstOrDefault(s => s.EntityId == userSetting.EntityId && s.UserId == LoggedInUser.Id);
            if (setting == null)
            {
                userSetting.CompanyId = LoggedInUser.CompanyId;
                userSetting.UserId = LoggedInUser.Id;
                _dbContext.Set<UserSetting>().Add(userSetting);
            }
            else
            {
                userSetting.CompanyId = LoggedInUser.CompanyId;
                setting.PageSize = userSetting.PageSize;
                _dbContext.Set<UserSetting>().Update(setting);
            }
            _dbContext.SaveChanges();
            return userSetting;
        }

        public int SaveAll(List<UserSetting> companies)
        {
            _dbContext.Set<UserSetting>().AddRange(companies);
            return _dbContext.SaveChanges();

        }

        public int ForPaging(PagedDataRequest requestInfo, EntityId id)
        {
            var pageSize = GetPageSize(id);
            if (requestInfo.PageSize == 0)
            {
                return 10;
            }
            if (pageSize == requestInfo.PageSize)
            {
                return pageSize;
            }
            var userSetting = new UserSetting
            {
                EntityId = id,
                PageSize = requestInfo.PageSize,
                Active = true
            };
            Save(userSetting);
            return requestInfo.PageSize;
        }
    }
}
