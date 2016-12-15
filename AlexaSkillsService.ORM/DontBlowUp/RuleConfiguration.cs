using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.DontBlowUp
{
    internal class RuleConfiguration : EntityTypeConfiguration<Rule>
    {
        public RuleConfiguration()
        {
            ToTable("Rule", "DontBlowUp");
        }
    }
}
