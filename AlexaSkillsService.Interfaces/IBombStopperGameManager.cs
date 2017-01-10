using AlexaSkillsService.Models.BombStopper;

namespace AlexaSkillsService.Interfaces
{
    public interface IBombStopperGameManager
    {
        Game CreateGame(string sessionId, string userId);

        Game GetGameBySerialNumber(string serialNumber, double minutesToOpenGame);

        Game GetGameById(int gameId);

        Game StartGame(int gameId, double minutesToOpenGame);

        Game Solve(int gameId, int wireToCut);
    }
}