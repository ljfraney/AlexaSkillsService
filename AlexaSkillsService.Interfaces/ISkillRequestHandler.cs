using AlexaSkillsService.Models;
using System.Threading.Tasks;

namespace AlexaSkillsService.Interfaces
{
    public interface ISkillRequestHandler
    {
        Task<AlexaResponse> HandleLaunchRequest(AlexaRequest request, AlexaResponse response);

        Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response);

        Task<AlexaResponse> HandleSessionEndedRequest(AlexaRequest request, AlexaResponse response);
    }
}
