using System;
using System.Collections.Generic;

namespace AlexaSkillsService.Data.DontBlowUp
{
    // ReSharper disable once DoNotCallOverridableMethodsInConstructor
    public class Game
    {
        public Game()
        {
            Rules = new HashSet<Rule>();
        }

        public int GameId { get; set; }

        public int SerialNumber { get; set; }

        public int NumberOfWires { get; set; }

        public string SessionId { get; set; }

        public string UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleted { get; set; }

        public int NarrativeId { get; set; }

        //FallThroughWirePosition should never be greater than NumberOfWires.
        public int FallThroughWirePosition { get; set; }

        public string FallThroughRuleText { get; set; }

        public virtual Narrative Narrative { get; set; }

        public virtual ICollection<Rule> Rules { get; set; }
    }
}
