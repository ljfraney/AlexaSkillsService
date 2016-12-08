using Newtonsoft.Json;

namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    [JsonObject("response")]
    public class Response
    {
        [JsonProperty("outputSpeech")]
        public OutputSpeech OutputSpeech { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("reprompt")]
        public Reprompt Reprompt { get; set; }

        [JsonProperty("shouldEndSession")]
        public bool ShouldEndSession { get; set; }

        public Response()
        {
            OutputSpeech = new OutputSpeech();
            Card = new Card();
            Reprompt = new Reprompt();
            ShouldEndSession = false;
        }
    }
}