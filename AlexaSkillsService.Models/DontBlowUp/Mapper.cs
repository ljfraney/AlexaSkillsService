using System.Linq;

namespace AlexaSkillsService.Models.DontBlowUp
{
    // ReSharper disable MemberCanBePrivate.Global
    public static class Mapper
    {
        public static Game ToModel(this Data.DontBlowUp.Game dbGame)
        {
            return new Game
            {
                GameId = dbGame.GameId,
                SerialNumber = dbGame.SerialNumber,
                TimeCreated = dbGame.TimeCreated,
                SecondsToSolve = dbGame.SecondsToSolve,
                TimeStarted = dbGame.TimeStarted,
                TimeCompleted = dbGame.TimeCompleted,
                Won = dbGame.Won,
                TimeRanOut = dbGame.TimeRanOut,
                Wires = dbGame.Wires.Select(w => w.ToModel()).ToList(),
                Narrative = dbGame.Narrative?.Text,
                RuleSets = dbGame.RuleSets.Select(rs => rs.ToModel()).ToList()
            };
        }

        public static Wire ToModel(this Data.DontBlowUp.Wire dbWire)
        {
            return new Wire
            {
                SortOrder = dbWire.SortOrder,
                Color = dbWire.Color
            };
        }

        public static RuleSet ToModel(this Data.DontBlowUp.RuleSet dbRuleSet)
        {
            return new RuleSet
            {
                NumberOfWires = dbRuleSet.NumberOfWires,
                FallThroughWirePosition = dbRuleSet.FallThroughWirePosition,
                FallThroughRuleText = dbRuleSet.FallThroughRuleText,
                Rules = dbRuleSet.Rules.Select(r => r.ToModel()).ToList()
            };
        }

        public static Rule ToModel(this Data.DontBlowUp.Rule dbRule)
        {
            return new Rule
            {
                RuleIndex = dbRule.RuleIndex,
                IsPosition = dbRule.IsPosition,
                WireColor = dbRule.WireColor,
                Operator = dbRule.Operator,
                RuleText = dbRule.RuleText,
                WirePositionOrCount = dbRule.WirePositionOrCount,
                WireToCutPosition = dbRule.WireToCutPosition
            };
        }
    }
}
