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
            var randomNarrativeId = RandomHelper.Instance.Next(1, maxNarrativeId + 1);
            var narrative = _alexaSkillsContext.Narratives.Single(n => n.NarrativeId == randomNarrativeId);

            var game = new Game
            {
                SerialNumber = RandomHelper.Instance.Next(10000, 100000), //Max will be 99999
                NumberOfWires = RandomHelper.Instance.Next(minWireCount, maxWireCount + 1),
                NarrativeId = randomNarrativeId,
                DateCreated = DateTime.UtcNow,
                SessionId = sessionId,
                UserId = userId
            };
            
            for (var i = minWireCount; i <= maxWireCount; i++)
                game.RuleSets.Add(GetRandomRuleSet(i));

            _alexaSkillsContext.Games.Add(game);
            _alexaSkillsContext.SaveChanges();

            return new Models.DontBlowUp.Game
            {
                GameId = game.GameId,
                SerialNumber = game.SerialNumber,
                NumberOfWires = game.NumberOfWires,
                DateCreated = game.DateCreated,
                Narrative = narrative.Text,
                RuleSets = game.RuleSets.Select(rs => new Models.DontBlowUp.RuleSet {
                    NumberOfWires = rs.NumberOfWires,
                    FallThroughWirePosition = rs.FallThroughWirePosition,
                    FallThroughRuleText = rs.FallThroughRuleText,
                    Rules = rs.Rules.Select(r => new Models.DontBlowUp.Rule
                    {
                        RuleIndex = r.RuleIndex,
                        IsPosition = r.IsPosition,
                        WireColor = r.WireColor,
                        Operator = r.Operator,
                        RuleText = r.RuleText,
                        WirePositionOrCount = r.WirePositionOrCount,
                        WireToCutPosition = r.WireToCutPosition
                    }).ToList()
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
                .Include(g => g.RuleSets.Select(rs => rs.Rules))
                .Include(g => g.Narrative).FirstOrDefault();

            if (dbGame != null)
                return new Models.DontBlowUp.Game
                {
                    GameId = dbGame.GameId,
                    SerialNumber = dbGame.SerialNumber,
                    NumberOfWires = dbGame.NumberOfWires,
                    DateCreated = dbGame.DateCreated,
                    Narrative = dbGame.Narrative.Text,
                    RuleSets = dbGame.RuleSets.Select(rs => new Models.DontBlowUp.RuleSet
                    {
                        NumberOfWires = rs.NumberOfWires,
                        FallThroughWirePosition = rs.FallThroughWirePosition,
                        FallThroughRuleText = rs.FallThroughRuleText,
                        Rules = rs.Rules.Select(r => new Models.DontBlowUp.Rule
                        {
                            RuleIndex = r.RuleIndex,
                            IsPosition = r.IsPosition,
                            WireColor = r.WireColor,
                            Operator = r.Operator,
                            RuleText = r.RuleText,
                            WirePositionOrCount = r.WirePositionOrCount,
                            WireToCutPosition = r.WireToCutPosition
                        }).ToList()
                    }).ToList()
                };

            return null;
        }

        public RuleSet GetRandomRuleSet(int wireCount)
        {
            var ruleSet = new RuleSet
            {
                NumberOfWires = wireCount,
                FallThroughWirePosition = RandomHelper.Instance.Next(1, wireCount + 1)
            };

            //Set the text for the fall-through rule.
            if (ruleSet.FallThroughWirePosition == 1)
                ruleSet.FallThroughRuleText = "cut the first wire.";
            else if (ruleSet.FallThroughWirePosition == wireCount)
            {
                //If the fall-through wire position is the last wire, 50% of the time it will read "cut the last wire",
                //and 50% of the time, it will say "cut the Nth wire".
                if (RandomHelper.Instance.Next(0, 2) == 0)
                    ruleSet.FallThroughRuleText = "cut the last wire.";
                else
                    ruleSet.FallThroughRuleText = $"cut the {ruleSet.FallThroughWirePosition.GetOrdinal()} wire.";
            }
            else
                ruleSet.FallThroughRuleText = $"cut the {ruleSet.FallThroughWirePosition.GetOrdinal()} wire.";
            
            var wireColorList = Enum.GetValues(typeof(WireColor)).Cast<int>().ToList();
            wireColorList.Shuffle();

            for (var ruleIndex = 0; ruleIndex < 5; ruleIndex++)
            {
                var rule = new Rule
                {
                    RuleIndex = ruleIndex,
                    WireColor = (WireColor)wireColorList[ruleIndex],
                    IsPosition = RandomHelper.Instance.Next(0, 2) == 0,
                    WireToCutPosition = RandomHelper.Instance.Next(1, wireCount + 1)
                };

                var ruleText = new StringBuilder();

                //If IsPosition is true, then the value of WirePositionOrCount must be greater than zero, and not greater than NumberOfWires.
                //If IsPosition is false, then the value of WirePositionOrCount must be zero or greater, but not more than the ceiling of 50% 
                //of NumberOfWires. This way we will never have a rule that says "If there are 6 black wires..."
                if (rule.IsPosition)
                {
                    rule.WirePositionOrCount = RandomHelper.Instance.Next(1, wireCount + 1);
                    ruleText.Append($"the {rule.WirePositionOrCount.GetOrdinal()} wire is {rule.WireColor.ToString().ToLower()}, ");
                }
                else
                {
                    rule.WirePositionOrCount = RandomHelper.Instance.Next(0, (int)Math.Ceiling(wireCount/2.0) + 1);

                    if (rule.WirePositionOrCount > 1) //Set a random operator (<, <=, ==, >=, >) if the count is greater than one.
                        rule.Operator = (WireCountOperator)RandomHelper.Instance.Next(1, Enum.GetValues(typeof(WireCountOperator)).Length + 1);
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
                else if (rule.WireToCutPosition == wireCount)
                {
                    //If the fall-through wire position is the last wire, 50% of the time it will read "cut the last wire",
                    //and 50% of the time, it will say "cut the Nth wire".
                    if (RandomHelper.Instance.Next(0, 2) == 0)
                        ruleText.Append("cut the last wire.");
                    else
                        ruleText.Append($"cut the {rule.WireToCutPosition.GetOrdinal()} wire.");
                }
                else
                    ruleText.Append($"cut the {rule.WireToCutPosition.GetOrdinal()} wire.");

                rule.RuleText = ruleText.ToString();
                ruleSet.Rules.Add(rule);
            }

            return ruleSet;
        }
    }
}
