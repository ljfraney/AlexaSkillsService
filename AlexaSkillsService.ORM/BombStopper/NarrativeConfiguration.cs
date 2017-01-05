using AlexaSkillsService.Data.BombStopper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.BombStopper
{
    internal class NarrativeConfiguration : EntityTypeConfiguration<Narrative>
    {
        public NarrativeConfiguration()
        {
            ToTable("Narrative", "BombStopper");

            Property(n => n.Key)
                .HasMaxLength(25)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(n => n.Text)
                .HasColumnType("nvarchar(max)");
        }
    }
}
