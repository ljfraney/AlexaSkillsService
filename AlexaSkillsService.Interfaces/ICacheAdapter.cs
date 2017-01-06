namespace AlexaSkillsService.Interfaces
{
    public interface ICacheAdapter
    {
        void Add<T>(string key, T value, int minutesToExpire) where T : class;

        T Get<T>(string key) where T : class;
    }
}