using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlexaSkillsService.Models
{
    public class Attributes
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable UnusedMember.Global
        public SkillAttributes SkillAttributes { get; set; }

        public List<string> Strings { get; set; } 

        public List<KeyValuePair<string, string>> KeyValuePairs { get; set; }

        public Attributes()
        {
            SkillAttributes = new SkillAttributes();
            Strings = new List<string>();
            KeyValuePairs = new List<KeyValuePair<string, string>>();
        }
    }
}