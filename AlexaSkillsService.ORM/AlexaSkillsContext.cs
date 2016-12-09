using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AlexaSkillsService.ORM
{
    public class AlexaSkillsContext : DbContext
    {
        public AlexaSkillsContext() : base("name=AlexaSkills")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual IDbSet<Language> Languages { get; set; }

        public virtual IDbSet<YearRange> LanguageYearRanges { get; set; }
        
        public virtual IDbSet<WireColor> WireColors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Language>().ToTable("Language", "DontBlowUp");
        }  
    }
}