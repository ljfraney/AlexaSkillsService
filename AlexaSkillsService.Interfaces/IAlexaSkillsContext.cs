using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AlexaSkillsService.Interfaces
{
    public interface IAlexaSkillsContext
    {
        IDbSet<Game> Games { get; set; }

        IDbSet<Narrative> Narratives { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
