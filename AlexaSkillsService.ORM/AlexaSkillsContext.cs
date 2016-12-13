using AlexaSkillsService.Data.DontBlowUp;
using AlexaSkillsService.Interfaces;
using AlexaSkillsService.ORM.DontBlowUp;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AlexaSkillsService.ORM
{
    public class AlexaSkillsContext : DbContext, IAlexaSkillsContext
    {
        public AlexaSkillsContext() : base("name=AlexaSkills")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual IDbSet<Game> Games { get; set; } 

        public virtual IDbSet<Narrative> Narratives { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new NarrativeConfiguration());
        }  
    }
}