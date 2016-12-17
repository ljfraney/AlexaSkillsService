using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.DontBlowUp
{
    public class WireConfiguration : EntityTypeConfiguration<Wire>
    {
        public WireConfiguration()
        {
            ToTable("Wire", "DontBlowUp");

            HasKey(w => new { w.GameId, w.SortOrder });
        }
    }
}
