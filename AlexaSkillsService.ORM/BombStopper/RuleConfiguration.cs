using AlexaSkillsService.Data.BombStopper;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.BombStopper
{
    internal class RuleConfiguration : EntityTypeConfiguration<Rule>
    {
        public RuleConfiguration()
        {
            ToTable("Rule", "BombStopper");
        }
    }
}
