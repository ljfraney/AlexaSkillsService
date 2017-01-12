using AlexaSkillsService.Common;
using AlexaSkillsService.Filters;
using AlexaSkillsService.Interfaces;
using AlexaSkillsService.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AlexaSkillsService.Utilities;
using RedisCacheManager;

namespace AlexaSkillsService.Controllers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [UnhandledExceptionFilter]
    [RoutePrefix("api/alexa")]
    public class AlexaController : ApiController
    {
        private const double TimeStampTolerance = 150;
        private const int DateMaxInDays = 60; //example of timeframe you might to check for
        private const int CacheExpireMinutes = 5;

        private readonly IAlexaSkillsContext _alexaSkillsContext;
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly ICacheAdapter _cacheAdapter;

        //BombStopper, etc.
        private ISkillRequestHandler _skillRequestHandler;

        public AlexaController(IAlexaSkillsContext alexaSkillsContext, IConfigurationAdapter configurationAdapter, ICacheAdapter cacheAdapter)
        {
            _alexaSkillsContext = alexaSkillsContext;
            _configurationAdapter = configurationAdapter;
            _cacheAdapter = cacheAdapter;
        }

        [HttpPost, Route("main")]
        public async Task<AlexaResponse> Main(AlexaRequest alexaRequest)
        {
            var response = new AlexaResponse();

            try
            {
                //check timestamp
                var totalSeconds = (DateTime.UtcNow - alexaRequest.Request.Timestamp).TotalSeconds;
                if (totalSeconds >= TimeStampTolerance)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

                if (alexaRequest.Session.Application.ApplicationId == _configurationAdapter.BombStopperAppId)
                    _skillRequestHandler = new BombStopperRequestHandler(new BombStopperGameManager(_alexaSkillsContext, _configurationAdapter), _configurationAdapter);
                else
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

                response.SessionAttributes.SkillAttributes = alexaRequest.Session.Attributes.SkillAttributes;

                switch (alexaRequest.Request.Type)
                {
                    case "LaunchRequest":
                        response = await _skillRequestHandler.HandleLaunchRequest(alexaRequest, response);
                        break;
                    case "IntentRequest":
                        response = await _skillRequestHandler.HandleIntentRequest(alexaRequest, response);
                        break;
                    case "SessionEndedRequest":
                        response = await _skillRequestHandler.HandleSessionEndedRequest(alexaRequest, response);
                        break;
                    default:
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
                }

                //set value for repeat intent
                response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
            }
            catch (Exception ex)
            {
                return await SafeExceptionHandler.ThrowSafeException(ex, response);
            }

            return response;
        }
    }
}