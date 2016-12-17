namespace AlexaSkillsService.Interfaces
{
    public interface IDontBlowUpGameManager
    {
        Models.DontBlowUp.Game CreateGame(string sessionId, string userId);

        Models.DontBlowUp.Game StartGame(string serialNumber, double minutesToOpenGame);

        Models.DontBlowUp.Game Solve(int gameId, int wireToCut);
    }
}