using System.Collections.Generic;

namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable ClassNeverInstantiated.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    public class SkillAttributes
    {
        public int SkillState { get; set; }

        public List<KeyValuePair<string, string>> KeyValuePairs { get; set; }
        
        public OutputSpeech OutputSpeech { get; set; }

        public SkillAttributes()
        {
            OutputSpeech = new OutputSpeech();
            KeyValuePairs = new List<KeyValuePair<string, string>>();
        }
    }
}