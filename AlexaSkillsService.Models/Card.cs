using Newtonsoft.Json;

namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    [JsonObject("card")]
    public class Card
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        public Card()
        {
            Type = "Simple";
            Image = new Image();
        }
    }
}