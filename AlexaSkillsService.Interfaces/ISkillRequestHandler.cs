using AlexaSkillsService.Models;
using System.Threading.Tasks;

namespace AlexaSkillsService.Interfaces
{
    public interface ISkillRequestHandler
    {
        AlexaResponse HandleLaunchRequest(AlexaRequest request, AlexaResponse response);

        Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response);

        AlexaResponse HandleSessionEndedRequest(AlexaRequest request, AlexaResponse response);
    }
}
