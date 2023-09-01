using Hrevert.Common.Security;
using HrevertCRM.Entities;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HrevertCRM.Data.QueryProcessors
{
    public class SlideSettingQueryProcessor : QueryBase<SlideSetting>, ISlideSettingQueryProcessor
    {
        private readonly IHostingEnvironment _env;
        public SlideSettingQueryProcessor(IUserSession userSession, IDbContext dbContext, IHostingEnvironment env)
            : base(userSession, dbContext)
        {
            _env = env;
        }
        public SlideSetting GetSlideSetting(int slideSettingId)
        {
            var slide = _dbContext.Set<SlideSetting>().FirstOrDefault(x => x.Id == slideSettingId && x.CompanyId==LoggedInUser.CompanyId && x.Active);
            return slide;
        }

        public SlideSetting Save(SlideSetting slideSetting)
        {
            slideSetting.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SlideSetting>().Add(slideSetting);
            _dbContext.SaveChanges();
            return slideSetting;
        }

        public SlideSetting Get()
        {
            return _dbContext.Set<SlideSetting>().Include(x=>x.IndividualSlideSettings).FirstOrDefault(x => x.CompanyId == LoggedInUser.CompanyId && x.Active);
        }
        public SlideSetting Update(SlideSetting slideSetting)
        {
            var original = GetSlideSetting(slideSetting.Id);
            ValidateAuthorization(original);
            CheckVersionMismatch(slideSetting, original);
            //pass value to original
            original.NumberOfSlides = slideSetting.NumberOfSlides;          
            original.CompanyId = LoggedInUser.CompanyId;
            _dbContext.Set<SlideSetting>().Update(original);
            _dbContext.SaveChanges();
            return original;
        }
    }
}
