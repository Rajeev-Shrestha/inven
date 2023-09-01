using Hrevert.Common.Enums;
using System.Collections.Generic;

namespace HrevertCRM.Entities
{
    public class SlideSetting : BaseEntity
    {
        public int Id { get; set; }
        //Slide Options
        //Configure your slides here

        public int NumberOfSlides { get; set; } //Max allowed 5
        public virtual ICollection<IndividualSlideSetting> IndividualSlideSettings { get; set; }
    }
}
