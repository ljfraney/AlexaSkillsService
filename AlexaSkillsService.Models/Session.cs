namespace AlexaSkillsService.Models
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    public class Session
    {
        public bool New { get; set; }

        public string SessionId { get; set; }
        
        public Application Application { get; set; }
        
        public Attributes Attributes { get; set; }

        public User User { get; set; }

        public Session()
        {
            Application = new Application();
            Attributes = new Attributes();
            User = new User();
        }
    }
}