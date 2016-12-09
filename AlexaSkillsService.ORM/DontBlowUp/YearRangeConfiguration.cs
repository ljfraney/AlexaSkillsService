using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.DontBlowUp
{
    public class YearRangeConfiguration : EntityTypeConfiguration<YearRange>
    {
        public YearRangeConfiguration()
        {
            ToTable("YearRange", "DontBlowUp");
        }
    }
}
