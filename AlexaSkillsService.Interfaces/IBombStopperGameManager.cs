namespace AlexaSkillsService.Interfaces
{
    public interface IBombStopperGameManager
    {
        Models.BombStopper.Game CreateGame(string sessionId, string userId);

        Models.BombStopper.Game StartGame(string serialNumber, double minutesToOpenGame);

        Models.BombStopper.Game Solve(int gameId, int wireToCut);
    }
}