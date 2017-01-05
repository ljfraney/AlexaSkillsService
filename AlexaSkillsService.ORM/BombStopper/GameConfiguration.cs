using AlexaSkillsService.Data.BombStopper;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.BombStopper
{
    internal class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            ToTable("Game", "BombStopper");

            Property(g => g.SessionId)
                .HasMaxLength(150)
                .IsUnicode(false);

            Property(g => g.UserId)
                .HasMaxLength(512)
                .IsUnicode(false);
        }
    }
}