namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    public class AlexaRequest
    {
        public string Version { get; set; }

        public Session Session { get; set; }

        public Request Request { get; set; }

        public AlexaRequest()
        {
            Version = "1.0";
            Session = new Session();
            Request = new Request();
        }
    }
}