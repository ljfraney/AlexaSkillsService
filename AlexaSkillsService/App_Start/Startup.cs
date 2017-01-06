using Microsoft.AspNet.SignalR;
using Owin;
using System.Configuration;

namespace AlexaSkillsService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseRedis(new RedisScaleoutConfiguration(ConfigurationManager.AppSettings["RedisCache"], "AlexaSkillsService"));
            app.MapSignalR();
        }
    }
}