using System.Configuration;

namespace AlexaSkillsService.Helpers
{
    public static class AppSettings
    {
        public static string RedisCache = ConfigurationManager.AppSettings["RedisCache"];

        public static string DontBlowUpAppId = ConfigurationManager.AppSettings["DontBlowUpAppId"];
    }
}