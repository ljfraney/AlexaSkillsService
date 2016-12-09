using AlexaSkillsService.Data.DontBlowUp;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace AlexaSkillsService.ORM.DontBlowUp
{
    public class WireColorConfiguration : EntityTypeConfiguration<WireColor>
    {
        public WireColorConfiguration()
        {
            ToTable("WireColor", "DontBlowUp");

            Property(l => l.Name)
                .HasMaxLength(25)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
        }
    }
}
