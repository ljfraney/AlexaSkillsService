using AlexaSkillsService.Utilities;
using Microsoft.Practices.Unity;
using RedisCacheManager;
using System;
using System.Linq;
using System.Web.Http;
using Unity.WebApi;

namespace AlexaSkillsService
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterInstance<IRedisManager>(new CacheManager(new StackExchangeCacher(new ConfigurationAdapter().RedisCache)));

            var assemblies = AllClasses.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).Where(a => a.Namespace != null && a.Namespace.Contains("AlexaSkillsService"));

            container.RegisterTypes(assemblies,
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.Transient);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}