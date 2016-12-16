using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.DontBlowUp
{
    internal class RuleSetConfiguration : EntityTypeConfiguration<RuleSet>
    {
        public RuleSetConfiguration()
        {
            ToTable("RuleSet", "DontBlowUp");

            HasKey(rs => new { rs.GameId, rs.NumberOfWires });
        }
    }
}
