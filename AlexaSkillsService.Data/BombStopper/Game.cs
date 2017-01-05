using System;
using System.Collections.Generic;

namespace AlexaSkillsService.Data.BombStopper
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    // ReSharper disable DoNotCallOverridableMethodsInConstructor
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
    public class Game
    {
        public Game()
        {
            RuleSets = new HashSet<RuleSet>();
            Wires = new HashSet<Wire>();
        }

        public int GameId { get; set; }

        public int SerialNumber { get; set; }

        public int Year { get; set; }

        public string SessionId { get; set; }

        public string UserId { get; set; }

        public DateTime TimeCreated { get; set; }

        public int SecondsToSolve { get; set; }

        public DateTime? TimeStarted { get; set; }

        public DateTime? TimeCompleted { get; set; }

        public bool? Won { get; set; }

        public bool? TimeRanOut { get; set; }

        public virtual ICollection<RuleSet> RuleSets { get; set; }

        public virtual ICollection<Wire> Wires { get; set; } 
    }
}
