using System.Collections.Generic;

namespace AlexaSkillsService.Data.DontBlowUp
{
    public class Language
    {
        public int LanguageId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<YearRange> YearRanges { get; set; }
    }
}
