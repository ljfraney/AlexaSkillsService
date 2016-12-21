﻿using AlexaSkillsService.Interfaces;
using AlexaSkillsService.Models.DontBlowUp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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

        public Game CreateGame(string sessionId, string userId)
        {
            const int minWireCount = 3;
            const int maxWireCount = 6;

            var maxNarrativeId = _alexaSkillsContext.Narratives.Max(n => n.NarrativeId);
            var randomNarrativeId = RandomHelper.Instance.Next(1, maxNarrativeId + 1);
            var narrative = _alexaSkillsContext.Narratives.Single(n => n.NarrativeId == randomNarrativeId);

            var game = new Data.DontBlowUp.Game
            {
                SerialNumber = RandomHelper.Instance.Next(10000, 100000), //Max will be 99999
                NarrativeId = randomNarrativeId,
                TimeCreated = DateTime.UtcNow,
                SecondsToSolve = 60,
                SessionId = sessionId,
                UserId = userId
            };

            //Add Wires
            var numberOfWires = RandomHelper.Instance.Next(minWireCount, maxWireCount + 1);
            for (var i = 1; i <= numberOfWires; i++)
            {
                var wire = new Data.DontBlowUp.Wire
                {
                    SortOrder = i,
                    Color = (Data.DontBlowUp.WireColor)RandomHelper.Instance.Next(1, Enum.GetValues(typeof(Data.DontBlowUp.WireColor)).Length + 1)
                };
                game.Wires.Add(wire);
            }
            
            for (var i = minWireCount; i <= maxWireCount; i++)
                game.RuleSets.Add(GetRandomRuleSet(i));

            _alexaSkillsContext.Games.Add(game);
            _alexaSkillsContext.SaveChanges();

            var gameModel = game.ToModel();
            gameModel.Narrative = narrative.Text;
            return gameModel;
        }

        public Game StartGame(string serialNumber, double minutesToOpenGame)
        {
            int iSerialNumber;
            if (!int.TryParse(serialNumber, out iSerialNumber))
                return null;

            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-minutesToOpenGame);
            var game = _alexaSkillsContext.Games
                .Where(g => g.SerialNumber == iSerialNumber && g.TimeCreated >= fiveMinutesAgo && g.TimeCompleted == null)
                .Include(g => g.RuleSets.Select(rs => rs.Rules))
                .Include(g => g.Narrative)
                .Include(g => g.Wires).FirstOrDefault();

            Debug.Assert(game != null, "game != null");
            game.TimeStarted = DateTime.UtcNow;
            _alexaSkillsContext.SaveChanges();

            return game?.ToModel();
        }
            
        private Data.DontBlowUp.RuleSet GetRandomRuleSet(int wireCount)
        {
            var ruleSet = new Data.DontBlowUp.RuleSet
            {
                NumberOfWires = wireCount,
                FallThroughWirePosition = RandomHelper.Instance.Next(1, wireCount + 1)
            };

            //Set the text for the fall-through rule.
            if (ruleSet.FallThroughWirePosition == 1)
                ruleSet.FallThroughRuleText = "cut the first wire";
            else if (ruleSet.FallThroughWirePosition == wireCount)
            {
                //If the fall-through wire position is the last wire, 50% of the time it will read "cut the last wire",
                //and 50% of the time, it will say "cut the Nth wire".
                if (RandomHelper.Instance.Next(0, 2) == 0)
                    ruleSet.FallThroughRuleText = "cut the last wire";
                else
                    ruleSet.FallThroughRuleText = $"cut the {ruleSet.FallThroughWirePosition.GetOrdinal()} wire";
            }
            else
                ruleSet.FallThroughRuleText = $"cut the {ruleSet.FallThroughWirePosition.GetOrdinal()} wire";
            
            var wireColorList = Enum.GetValues(typeof(Data.DontBlowUp.WireColor)).Cast<int>().ToList();
            wireColorList.Shuffle();

            for (var ruleIndex = 0; ruleIndex < 5; ruleIndex++)
            {
                var rule = new Data.DontBlowUp.Rule
                {
                    RuleIndex = ruleIndex,
                    WireColor = (Data.DontBlowUp.WireColor)wireColorList[ruleIndex],
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

                    if (rule.WirePositionOrCount == 0) //Only use == or > if the count is zero.
                    {
                        var validOperators = new List<Data.DontBlowUp.WireCountOperator> { Data.DontBlowUp.WireCountOperator.EqualTo, Data.DontBlowUp.WireCountOperator.GreaterThan };
                        validOperators.Shuffle();
                        rule.Operator = validOperators[0];
                    }
                    else if (rule.WirePositionOrCount == 1) //Only use ==, >=, or > if the count is one.
                    {
                        var validOperators = new List<Data.DontBlowUp.WireCountOperator> { Data.DontBlowUp.WireCountOperator.EqualTo, Data.DontBlowUp.WireCountOperator.GreaterThanOrEqualTo, Data.DontBlowUp.WireCountOperator.GreaterThan };
                        validOperators.Shuffle();
                        rule.Operator = validOperators[0];
                    }
                    else if (rule.WirePositionOrCount > 1) //Set a random operator (<, <=, ==, >=, >) if the count is greater than one.
                        rule.Operator = (Data.DontBlowUp.WireCountOperator)RandomHelper.Instance.Next(1, Enum.GetValues(typeof(Data.DontBlowUp.WireCountOperator)).Length + 1);

                    if (rule.WirePositionOrCount == 0)
                    {
                        switch (rule.Operator)
                        {
                            case Data.DontBlowUp.WireCountOperator.EqualTo:
                                ruleText.Append($"there are no {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case Data.DontBlowUp.WireCountOperator.GreaterThan:
                                ruleText.Append($"there are any {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                        }
                    }
                    else if (rule.WirePositionOrCount == 1)
                    {
                        switch (rule.Operator)
                        {
                            case Data.DontBlowUp.WireCountOperator.EqualTo:
                                ruleText.Append($"there is exactly one {rule.WireColor.ToString().ToLower()} wire, ");
                                break;
                            case Data.DontBlowUp.WireCountOperator.GreaterThanOrEqualTo:
                                ruleText.Append($"there are any {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case Data.DontBlowUp.WireCountOperator.GreaterThan:
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
                            case Data.DontBlowUp.WireCountOperator.LessThan:
                                ruleText.Append($"there are fewer than {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case Data.DontBlowUp.WireCountOperator.LessThanOrEqualTo:
                                ruleText.Append($"there are {rule.WirePositionOrCount} or less {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case Data.DontBlowUp.WireCountOperator.EqualTo:
                                ruleText.Append($"there are exactly {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case Data.DontBlowUp.WireCountOperator.GreaterThanOrEqualTo:
                                ruleText.Append($"there are at least {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            case Data.DontBlowUp.WireCountOperator.GreaterThan:
                                ruleText.Append($"there are more than {rule.WirePositionOrCount} {rule.WireColor.ToString().ToLower()} wires, ");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"An unexpected value for the WireCountOperator was encountered for a multiple wire rule where IsPosition is false: {rule.Operator}");
                        }
                    }
                }

                if (rule.WireToCutPosition == 1)
                    ruleText.Append("cut the first wire");
                else if (rule.WireToCutPosition == wireCount)
                {
                    //If the fall-through wire position is the last wire, 50% of the time it will read "cut the last wire",
                    //and 50% of the time, it will say "cut the Nth wire".
                    if (RandomHelper.Instance.Next(0, 2) == 0)
                        ruleText.Append("cut the last wire");
                    else
                        ruleText.Append($"cut the {rule.WireToCutPosition.GetOrdinal()} wire");
                }
                else
                    ruleText.Append($"cut the {rule.WireToCutPosition.GetOrdinal()} wire");

                rule.RuleText = ruleText.ToString();
                ruleSet.Rules.Add(rule);
            }

            return ruleSet;
        }

        public Game Solve(int gameId, int wireToCut)
        {
            var timeCompleted = DateTime.UtcNow;

            var game = _alexaSkillsContext.Games
                .Where(g => g.GameId == gameId)
                .Include(g => g.RuleSets.Select(rs => rs.Rules))
                .Include(g => g.Wires).Single();

            game.TimeCompleted = timeCompleted;

            Debug.Assert(game.TimeStarted != null, "game.TimeStarted != null");
            if ((timeCompleted - game.TimeStarted.Value).TotalSeconds > game.SecondsToSolve)
            {
                game.Won = false;
                game.TimeRanOut = true;
                _alexaSkillsContext.SaveChanges();
                return game.ToModel();
            }

            var ruleSet = game.RuleSets.Single(rs => rs.NumberOfWires == game.Wires.Count);

            var correctWire = 0;
            foreach (var rule in ruleSet.Rules)
            {
                if (rule.IsPosition)
                {
                    var wire = game.Wires.Single(w => w.SortOrder == rule.WirePositionOrCount);
                    if (wire.Color == rule.WireColor)
                    {
                        correctWire = rule.WireToCutPosition;
                        break;
                    }
                }
                else
                {
                    var wireCount = game.Wires.Count(w => w.Color == rule.WireColor);
                    if (rule.Operator == Data.DontBlowUp.WireCountOperator.LessThan && wireCount < rule.WirePositionOrCount)
                    {
                        correctWire = rule.WireToCutPosition;
                        break;
                    }
                    if (rule.Operator == Data.DontBlowUp.WireCountOperator.LessThanOrEqualTo && wireCount <= rule.WirePositionOrCount)
                    {
                        correctWire = rule.WireToCutPosition;
                        break;
                    }
                    if (rule.Operator == Data.DontBlowUp.WireCountOperator.EqualTo && wireCount == rule.WirePositionOrCount)
                    {
                        correctWire = rule.WireToCutPosition;
                        break;
                    }
                    if (rule.Operator == Data.DontBlowUp.WireCountOperator.GreaterThanOrEqualTo && wireCount >= rule.WirePositionOrCount)
                    {
                        correctWire = rule.WireToCutPosition;
                        break;
                    }
                    if (rule.Operator == Data.DontBlowUp.WireCountOperator.GreaterThan && wireCount > rule.WirePositionOrCount)
                    {
                        correctWire = rule.WireToCutPosition;
                        break;
                    }
                }
            }

            game.Won = wireToCut == correctWire;
            game.TimeRanOut = false;
            _alexaSkillsContext.SaveChanges();
            return game.ToModel();
        }
    }
}
