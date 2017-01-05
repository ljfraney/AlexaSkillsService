using System.Configuration;

namespace AlexaSkillsService.Helpers
{
    public static class AppSettings
    {
        public static string RedisCache = ConfigurationManager.AppSettings["RedisCache"];

        public static string BombStopperAppId = ConfigurationManager.AppSettings["BombStopperAppId"];
    }
}