namespace AlexaSkillsService.Models
{
    public class SkillAttributes
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable UnusedMember.Global
        public string LastRequestIntent { get; set; }

        public OutputSpeech OutputSpeech { get; set; }

        public SkillAttributes()
        {
            LastRequestIntent = "";
            OutputSpeech = new OutputSpeech();
        }
    }
}