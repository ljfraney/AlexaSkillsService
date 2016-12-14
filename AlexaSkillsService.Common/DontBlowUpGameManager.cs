using AlexaSkillsService.Data.DontBlowUp;
using AlexaSkillsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            var maxNarrativeId = _alexaSkillsContext.Narratives.Max(n => n.NarrativeId);
            var randomNarrativeId = RandomHelper.Instance.Next(1, maxNarrativeId);
            var narrative = _alexaSkillsContext.Narratives.Single(n => n.NarrativeId == randomNarrativeId);

            var game = new Game
            {
                SerialNumber = RandomHelper.Instance.Next(10000, 99999),
                NumberOfWires = RandomHelper.Instance.Next(4, 6),
                NarrativeId = randomNarrativeId,
                DateCreated = DateTime.UtcNow,
                SessionId = sessionId,
                UserId = userId
            };

            _alexaSkillsContext.Games.Add(game);
            _alexaSkillsContext.SaveChangesAsync();

            return new Models.DontBlowUp.Game
            {
                GameId = game.GameId,
                SerialNumber = game.SerialNumber,
                NumberOfWires = game.NumberOfWires,
                DateCreated = game.DateCreated,
                Narrative = narrative.Text
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
                .Include(g => g.Narrative).FirstOrDefault();

            if (dbGame != null)
                return new Models.DontBlowUp.Game
                {
                    GameId = dbGame.GameId,
                    SerialNumber = dbGame.SerialNumber,
                    DateCreated = dbGame.DateCreated,
                    Narrative = dbGame.Narrative.Text
                };

            return null;
        }

        private void GetRandomRuleSet(int numberOfWires)
        {
            var maxRule = Enum.GetValues(typeof(RuleType)).Cast<int>().Max();
            var rules = new List<int> { RandomHelper.Instance.Next(1, maxRule) };
            var nextRule = RandomHelper.Instance.Next(1, maxRule);
            var stop = false;

            while (!stop)
            {
                if (!rules.Contains(nextRule))
                    rules.Add(nextRule);

                if (rules.Count < numberOfWires - 1)
                    nextRule = RandomHelper.Instance.Next(1, maxRule);
                else
                    stop = true;
            }
            
            //TODO: Put together rules
        }
    }
}
