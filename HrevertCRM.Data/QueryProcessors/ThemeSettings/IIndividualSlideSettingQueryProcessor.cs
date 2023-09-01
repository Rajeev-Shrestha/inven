using System.Collections.Generic;
using HrevertCRM.Entities;

namespace HrevertCRM.Data.QueryProcessors
{
    public interface IIndividualSlideSettingQueryProcessor
    {
        IndividualSlideSetting Update(IndividualSlideSetting individualSlideSetting);
        void UpdateIndividualSlideSettings(List<IndividualSlideSetting> individualSlideSettings);
        IndividualSlideSetting GetIndividualSlideSetting(int individualSlideSettingId);
        IndividualSlideSetting Save(IndividualSlideSetting individualSlideSetting);
        void SaveIndividualSlideSettings(List<IndividualSlideSetting> individualSlideSettings);
    }
}
