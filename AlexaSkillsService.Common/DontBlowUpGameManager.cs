using AlexaSkillsService.Data.DontBlowUp;
using AlexaSkillsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace AlexaSkillsService.Common
{
    public class DontBlowUpGameManager : IDontBlowUpGameManager
    {
        private readonly IAlexaSkillsContext _alexaSkillsContext;

        public DontBlowUpGameManager(IAlexaSkillsContext alexaSkillsContext)
        {
            _alexaSkillsContext = alexaSkillsContext;
        }

        public Models.DontBlowUp.Game CreateGame(string sessionId, string userId)
        {
            const int minWireCount = 3;
            const int maxWireCount = 6;

            var maxNarrativeId = _alexaSkillsContext.Narratives.Max(n => n.NarrativeId);
            var randomNarrativeId = RandomHelper.Instance.Next(1, maxNarrativeId);
            var narrative = _alexaSkillsContext.Narratives.Single(n => n.NarrativeId == randomNarrativeId);

            var game = new Game
            {
                SerialNumber = RandomHelper.Instance.Next(10000, 99999),
                NumberOfWires = RandomHelper.Instance.Next(minWireCount, maxWireCount),
                NarrativeId = randomNarrativeId,
                DateCreated = DateTime.UtcNow,
                SessionId = sessionId,
                UserId = userId
            };

            game.FallThroughWirePosition = RandomHelper.Instance.Next(1, game.NumberOfWires);

            //Set the text for the fall-through rule.
            if (game.FallThroughWirePosition == 1)
                game.FallThroughRuleText = "cut the first wire.";
            else if (game.FallThroughWirePosition == game.NumberOfWires)
            {
                //If the fall-through wire position is the last wire, 50% of the time it will read "cut the last wire",
                //and 50% of the time, it will say "cut the Nth wire".
                if (RandomHelper.Instance.Next(0, 1) == 0)
                    game.FallThroughRuleText = "cut the last wire.";
                else
                    game.FallThroughRuleText = $"cut the {game.FallThroughWirePosition.GetOrdinal()} wire.";
            }
            else
                game.FallThroughRuleText = $"cut the {game.FallThroughWirePosition.GetOrdinal()} wire.";

            for (var i = minWireCount; i <= maxWireCount; i++)
            {
                var ruleSet = GetRandomRuleSet(i);
                foreach (var rule in ruleSet)
                    game.Rules.Add(rule);
            }

            _alexaSkillsContext.Games.Add(game);
            _alexaSkillsContext.SaveChanges();

            return new Models.DontBlowUp.Game
            {
                GameId = game.GameId,
                SerialNumber = game.SerialNumber,
                NumberOfWires = game.NumberOfWires,
                DateCreated = game.DateCreated,
                Narrative = narrative.Text,
                FallThroughWirePosition = game.FallThroughWirePosition,
                FallThroughRuleText = game.FallThroughRuleText,
                Rules = game.Rules.Select(r => new Models.DontBlowUp.Rule
                {
                    NumberOfWires = r.NumberOfWires,
                    RuleIndex = r.RuleIndex,
                    IsPosition = r.IsPosition,
                    WireColor = r.WireColor,
                    Operator = r.Operator,
                    RuleText = r.RuleText,
                    WirePositionOrCount = r.WirePositionOrCount,
                    WireToCutPosition = r.WireToCutPosition
                }).ToList()
            };
        }

        public Models.DontBlowUp.Game GetGameBySerialNumber(string serialNumber)
        {
            int iSerialNumber;
            if (!int.TryParse(serialNumber, out iSerialNumber))
                return null;

            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5.0);
            var dbGame = _alexaSkillsContext.Games
                .Where(g => g.SerialNumber == iSerialNumber && g.DateCreated >= fiveMinutesAgo && g.DateCompleted == null)
                .Include(g => g.Rules)
                .Include(g => g.Narrative).FirstOrDefault();

            if (dbGame != null)
                return new Models.DontBlowUp.Game
                {
                    GameId = dbGame.GameId,
                    SerialNumber = dbGame.SerialNumber,
                    NumberOfWires = dbGame.NumberOfWires,
                    DateCreated = dbGame.DateCreated,
                    Narrative = dbGame.Narrative.Text,
                    FallThroughWirePosition = dbGame.FallThroughWirePosition,
                    FallThroughRuleText = dbGame.FallThroughRuleText,
                    Rules = dbGame.Rules.Select(r => new Models.DontBlowUp.Rule
                    {
                        NumberOfWires = r.NumberOfWires,
                        RuleIndex = r.RuleIndex,
                        IsPosition = r.IsPosition,
                        WireColor = r.WireColor,
                        Operator = r.Operator,
                        RuleText = r.RuleText,
                        WirePositionOrCount = r.WirePositionOrCount,
                        WireToCutPosition = r.WireToCutPosition
                    }).ToList()
                };

            return null;
        }

        public List<Rule> GetRandomRuleSet(int wireCount)
        {
            var rules = new List<Rule>();
            var wireColorList = Enum.GetValues(typeof(WireColor)).Cast<int>().ToList();
            wireColorList.Shuffle();

            for (var ruleIndex = 0; ruleIndex < 5; ruleIndex++)
            {
                var rule = new Rule
                {
                    NumberOfWires = wireCount,
                    RuleIndex = ruleIndex,
                    WireColor = (WireColor)wireColorList[ruleIndex],
                    IsPosition = RandomHelper.Instance.Next(0, 1) == 0,
                    WireToCutPosition = RandomHelper.Instance.Next(1, wireCount)
                };

                var ruleText = new StringBuilder();

                //If IsPosition is true, then the value of WirePositionOrCount must be greater than zero, and not greater than NumberOfWires.
                //If IsPosition is false, then the value of WirePositionOrCount must be zero or greater, but not more than the ceiling of 50% 
                //of NumberOfWires. This way we will never have a rule that says "If there are 6 black wires..."
                if (rule.IsPosition)
                {
                    rule.WirePositionOrCount = RandomHelper.Instance.Next(1, wireCount);
                    ruleText.Append($"the {rule.WirePositionOrCount.GetOrdinal()} wire is {rule.WireColor.ToString().ToLower()}, ");
                }
                else
                {
                    rule.WirePositionOrCount = RandomHelper.Instance.Next(0, (int)Math.Ceiling(wireCount/2.0));

                    if (rule.WirePositionOrCount > 1) //Set a random operator (<, <=, ==, >=, >) if the count is greater than one.
                        rule.Operator = (WireCountOperator)RandomHelper.Instance.Next(1, Enum.GetValues(typeof(WireCountOperator)).Length);
                    else if (rule.WirePositionOrCount == 1) //Only use ==, >=, or > if the count is one.
                    {
                        var validOperators = new List<WireCountOperator> { WireCountOperator.EqualTo, WireCountOperator.GreaterThanOrEqualTo, WireCountOperator.GreaterThan };
                        validOperators.Shuffle();
                        rule.Operator = validOperators[0];
                    }

                    //Don't set an operator if the count is zero.

                    if (rule.WirePositionOrCount == 0)
                        ruleText.Append($"there are no {rule.WireColor.ToString().ToLower()} wires, ");
                    else if (rule.WirePositionOrCount == 1)
                    {
                        switch (rule.Operator)
                        {
                            case WireCountOperator.EqualTo:
                                ruleText.Append($"there is exactly one {rule.WireColor.ToString().ToLower()} wire, ");
                                break;
                            case WireCountOperator.GreaterThanOrEqualTo:
                                ruleText.Append($"there are one or more {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case WireCountOperator.GreaterThan:
                                ruleText.Append($"there is more than one {rule.WireColor.ToString().ToLower()} wire, ");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"An unexpected value for the WireCountOperator was encountered for a one wire rule where IsPosition is false: {rule.Operator}");
                        }
                    }
                    else
                    {
                        switch (rule.Operator)
                        {
                            case WireCountOperator.LessThan:
                                ruleText.Append($"there are fewer than {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case WireCountOperator.LessThanOrEqualTo:
                                ruleText.Append($"there are {rule.WirePositionOrCount} or less {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case WireCountOperator.EqualTo:
                                ruleText.Append($"there are exactly {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case WireCountOperator.GreaterThanOrEqualTo:
                                ruleText.Append($"there are at least {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case WireCountOperator.GreaterThan:
                                ruleText.Append($"there are more than {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"An unexpected value for the WireCountOperator was encountered for a multiple wire rule where IsPosition is false: {rule.Operator}");
                        }
                    }
                }

                if (rule.WireToCutPosition == 1)
                    ruleText.Append("cut the first wire.");
                else if (rule.WireToCutPosition == rule.NumberOfWires)
                {
                    //If the fall-through wire position is the last wire, 50% of the time it will read "cut the last wire",
                    //and 50% of the time, it will say "cut the Nth wire".
                    if (RandomHelper.Instance.Next(0, 1) == 0)
                        ruleText.Append("cut the last wire.");
                    else
                        ruleText.Append($"cut the {rule.WireToCutPosition.GetOrdinal()} wire.");
                }
                else
                    ruleText.Append($"cut the {rule.WireToCutPosition.GetOrdinal()} wire.");

                rule.RuleText = ruleText.ToString();
                rules.Add(rule);
            }

            return rules;
        }
    }
}
