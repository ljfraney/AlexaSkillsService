using Newtonsoft.Json;

namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    [JsonObject("outputSpeech")]
    public class OutputSpeech
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("ssml")]
        public string Ssml { get; set; }

        public OutputSpeech()
        {
            Type = "PlainText";
        }
    }
}