using System;
using System.Collections.Generic;

namespace AlexaSkillsService.Models.DontBlowUp
{
    public class Game
    {
        public int GameId { get; set; }

        public int SerialNumber { get; set; }

        public int NumberOfWires { get; set; }

        public DateTime DateCreated { get; set; }

        public string Narrative { get; set; }

        public List<Rule> Rules { get; set; }

        public int FallThroughWirePosition { get; set; }

        public string FallThroughRuleText { get; set; }
    }
}
