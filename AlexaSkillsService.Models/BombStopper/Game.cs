using System;
using System.Collections.Generic;

namespace AlexaSkillsService.Models.BombStopper
{
    public class Game
    {
        public int GameId { get; set; }

        public string CryptoGameId { get; set; }

        public int SerialNumber { get; set; }

        public int Year { get; set; }

        public DateTime TimeCreated { get; set; }

        public int SecondsToSolve { get; set; }

        public DateTime? TimeStarted { get; set; }

        public List<RuleSet> RuleSets { get; set; }

        public List<Wire> Wires { get; set; }

        public bool? Won { get; set; }

        public bool? TimeRanOut { get; set; }

        public DateTime? TimeCompleted { get; set; }
    }
}