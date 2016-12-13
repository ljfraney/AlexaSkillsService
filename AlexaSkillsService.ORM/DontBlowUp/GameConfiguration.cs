using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.DontBlowUp
{
    internal class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            ToTable("Game", "DontBlowUp");

            Property(g => g.SessionId)
                .HasMaxLength(150)
                .IsUnicode(false);

            Property(g => g.UserId)
                .HasMaxLength(512)
                .IsUnicode(false);
        }
    }
}