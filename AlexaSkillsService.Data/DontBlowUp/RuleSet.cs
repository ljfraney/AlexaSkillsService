using System.Collections.Generic;

namespace AlexaSkillsService.Data.DontBlowUp
{
    // ReSharper disable once DoNotCallOverridableMethodsInConstructor
    public class RuleSet
    {
        public RuleSet()
        {
            Rules = new HashSet<Rule>();
        }

        public int GameId { get; set; }

        //NumberOfWires should be greater than zero, and not greater than NumberOfWires on the Game.
        public int NumberOfWires { get; set; }

        //FallThroughWirePosition should never be greater than NumberOfWires.
        public int FallThroughWirePosition { get; set; }

        public string FallThroughRuleText { get; set; }

        public virtual ICollection<Rule> Rules { get; set; }
    }  
}
