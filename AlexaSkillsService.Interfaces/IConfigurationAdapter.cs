namespace AlexaSkillsService.Interfaces
{
    public interface IConfigurationAdapter
    {
        string RedisCache { get; }

        string BombStopperAppId { get; }

        string SharedSecret { get; }
    }
}