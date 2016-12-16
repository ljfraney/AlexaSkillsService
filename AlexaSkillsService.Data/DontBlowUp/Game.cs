using System;
using System.Collections.Generic;

namespace AlexaSkillsService.Data.DontBlowUp
{
    // ReSharper disable once DoNotCallOverridableMethodsInConstructor
    public class Game
    {
        public Game()
        {
            RuleSets = new HashSet<RuleSet>();
        }

        public int GameId { get; set; }

        public int SerialNumber { get; set; }

        public int NumberOfWires { get; set; }

        public string SessionId { get; set; }

        public string UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleted { get; set; }

        public int NarrativeId { get; set; }

        public virtual Narrative Narrative { get; set; }

        public virtual ICollection<RuleSet> RuleSets { get; set; }
    }
}
