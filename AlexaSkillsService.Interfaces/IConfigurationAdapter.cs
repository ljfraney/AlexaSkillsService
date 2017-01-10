namespace AlexaSkillsService.Interfaces
{
    public interface IConfigurationAdapter
    {
        string BombStopperAppId { get; }

        string SharedSecret { get; }

        int YearQuestionMaxTries { get; }
    }
}