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
        //This class maps to the 
        public string LastRequestIntent { get; set; }

        public int SkillState { get; set; }

        public List<KeyValuePair<string, string>> KeyValuePairs { get; set; }
        
        public OutputSpeech OutputSpeech { get; set; }

        public SkillAttributes()
        {
            LastRequestIntent = "";
            OutputSpeech = new OutputSpeech();
            KeyValuePairs = new List<KeyValuePair<string, string>>();
        }
    }
}