using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface ISlideSettingQueryProcessor
    {
        SlideSetting Update(SlideSetting slideSetting);
        SlideSetting GetSlideSetting(int slideSettingId);
        SlideSetting Save(SlideSetting slideSetting);
        SlideSetting Get();

    }
}
