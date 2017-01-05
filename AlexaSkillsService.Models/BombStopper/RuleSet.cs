using System.Collections.Generic;

namespace AlexaSkillsService.Models.BombStopper
{
    public class RuleSet
    {
        public int NumberOfWires { get; set; }

        public List<Rule> Rules { get; set; }

        public int FallThroughWirePosition { get; set; }

        public string FallThroughRuleText { get; set; }
    }
}