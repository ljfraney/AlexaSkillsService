using AlexaSkillsService.Data.BombStopper;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.BombStopper
{
    internal class RuleSetConfiguration : EntityTypeConfiguration<RuleSet>
    {
        public RuleSetConfiguration()
        {
            ToTable("RuleSet", "BombStopper");

            HasKey(rs => new { rs.GameId, rs.NumberOfWires });
        }
    }
}
