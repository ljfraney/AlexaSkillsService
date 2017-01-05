using AlexaSkillsService.Data.BombStopper;
using AlexaSkillsService.Interfaces;
using AlexaSkillsService.ORM.BombStopper;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AlexaSkillsService.ORM
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class AlexaSkillsContext : DbContext, IAlexaSkillsContext
    {
        public AlexaSkillsContext() : base("name=AlexaSkills")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual IDbSet<Game> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new RuleConfiguration());
            modelBuilder.Configurations.Add(new RuleSetConfiguration());
            modelBuilder.Configurations.Add(new WireConfiguration());
        }  
    }
}