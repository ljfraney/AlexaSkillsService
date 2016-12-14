namespace AlexaSkillsService.Interfaces
{
    public interface IDontBlowUpGameManager
    {
        Models.DontBlowUp.Game CreateGame(string sessionId, string userId);

        Models.DontBlowUp.Game GetGameBySerialNumber(string serialNumber);
    }
}