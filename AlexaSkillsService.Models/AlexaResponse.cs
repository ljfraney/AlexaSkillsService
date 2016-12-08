using Newtonsoft.Json;

namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    [JsonObject]
    public class AlexaResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("sessionAttributes")]
        public Attributes SessionAttributes { get; set; }

        [JsonProperty("response")]
        public Response Response { get; set; }

        public AlexaResponse()
        {
            Version = "1.0";
            SessionAttributes = new Attributes();
            Response = new Response();
        }

        public AlexaResponse(string outputSpeechText) : this()
        {
            Response.OutputSpeech.Text = outputSpeechText;
            Response.Card.Content = outputSpeechText;
        }

        public AlexaResponse(string outputSpeechText, bool isGoodbye) : this()
        {
            Response.OutputSpeech.Text = outputSpeechText;

            if (isGoodbye)
            {
                Response.ShouldEndSession = true;
                Response.Card = null;
            }
            else
                Response.Card.Content = outputSpeechText;
        }

        public AlexaResponse(string outputSpeechText, string cardContent) : this()
        {
            Response.OutputSpeech.Text = outputSpeechText;
            Response.Card.Content = cardContent;
        }
    }
}