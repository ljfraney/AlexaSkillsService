using AlexaSkillsService.Common;
using AlexaSkillsService.Filters;
using AlexaSkillsService.Helpers;
using AlexaSkillsService.Interfaces;
using AlexaSkillsService.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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

        //Using custom object to manage 
        private RedisCacheManager.IRedisManager _cache;

        //BombStopper, etc.
        private ISkillRequestHandler _skillRequestHandler;

        //Using cache for persiting important, yet non-permanent session related data. 
        //Examaples of getting and setting. Complex objects can be used as well:
        //_cache.Set<string>("key", "value", CacheExpiry);
        //_cache.Get<string>("key");
        public DateTime CacheExpiry => DateTime.Now.AddMinutes(CacheExpireMinutes);

        public AlexaController(IAlexaSkillsContext alexaSkillsContext)
        {
            _alexaSkillsContext = alexaSkillsContext;
            //_cache = new RedisCacheManager.CacheManager(new RedisCacheManager.StackExchangeCacher(AppSettings.RedisCache));
        }

        [HttpGet, Route("test")]
        public async Task<AlexaResponse> Test()
        {
            return new AlexaResponse("This is some output speech.");
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

                if (alexaRequest.Session.Application.ApplicationId == AppSettings.BombStopperAppId)
                    _skillRequestHandler = new BombStopperRequestHandler(new BombStopperGameManager(_alexaSkillsContext));
                else
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

                response.SessionAttributes.SkillAttributes = alexaRequest.Session.Attributes.SkillAttributes;

                switch (alexaRequest.Request.Type)
                {
                    case "LaunchRequest":
                        response = _skillRequestHandler.HandleLaunchRequest(alexaRequest, response);
                        break;
                    case "IntentRequest":
                        response = await _skillRequestHandler.HandleIntentRequest(alexaRequest, response);
                        break;
                    case "SessionEndedRequest":
                        response = _skillRequestHandler.HandleSessionEndedRequest(alexaRequest, response);
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