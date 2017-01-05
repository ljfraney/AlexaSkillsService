using AlexaSkillsService.Data.BombStopper;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AlexaSkillsService.Interfaces
{
    public interface IAlexaSkillsContext
    {
        IDbSet<Game> Games { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
