using Newtonsoft.Json;

namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    [JsonObject("reprompt")]
    public class Reprompt
    {
        [JsonProperty("outputSpeech")]
        public OutputSpeech OutputSpeech { get; set; }

        public Reprompt()
        {
            OutputSpeech = new OutputSpeech();
        }
    }
}