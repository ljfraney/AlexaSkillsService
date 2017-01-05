using AlexaSkillsService.Data.BombStopper;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.BombStopper
{
    public class WireConfiguration : EntityTypeConfiguration<Wire>
    {
        public WireConfiguration()
        {
            ToTable("Wire", "BombStopper");

            HasKey(w => new { w.GameId, w.SortOrder });
        }
    }
}
