using Microsoft.Practices.Unity;
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
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            var assemblies = AllClasses.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).Where(a => a.Namespace != null && a.Namespace.Contains("AlexaSkillsService"));

            container.RegisterTypes(assemblies,
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.Transient);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}