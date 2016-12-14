using System;

namespace AlexaSkillsService.Data.DontBlowUp
{
    public class Game
    {
        public int GameId { get; set; }

        public int SerialNumber { get; set; }

        public int NumberOfWires { get; set; }

        public string SessionId { get; set; }

        public string UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleted { get; set; }

        public int NarrativeId { get; set; }

        public virtual Narrative Narrative { get; set; }
    }
}
