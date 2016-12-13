using AlexaSkillsService.Common;
using AlexaSkillsService.Data.DontBlowUp;
using AlexaSkillsService.Interfaces;
using System;

namespace DontBlowUp
{
    public class GameManager
    {
        private readonly IAlexaSkillsContext _alexaSkillsContext;

        public GameManager(IAlexaSkillsContext alexaSkillsContext)
        {
            _alexaSkillsContext = alexaSkillsContext;
        }

        public int CreateGame(string sessionId, string userId)
        {
            var game = new Game
            {
                SerialNumber = RandomHelper.Instance.Next(00001, 99999),
                DateCreated = DateTime.UtcNow,
                SessionId = sessionId,
                UserId = userId
            };

            _alexaSkillsContext.Games.Add(game);
            _alexaSkillsContext.SaveChangesAsync();

            return game.SerialNumber;
        }
    }
}
