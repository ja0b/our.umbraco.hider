using LightInject;
using Our.Umbraco.Hider.Core.Components;
using Our.Umbraco.Hider.Core.Services;
using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.Hider.Core.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class UmbracoHiderStartupComposers : IUserComposer
    {
        public void Compose(Composition composition)
        {
            var container = composition.Concrete as ServiceContainer;

            RegisterServices(container);
            RegisterAssembliesControllers(container);

            composition.Components()
                .Append<UmbracoHiderStartupComponent>()
                .Append<EditorModelEventManagerComponent>();
        }

        private static void RegisterServices(IServiceRegistry container)
        {
            container.Register<IConfigurationService, ConfigurationService>();
        }

        private static void RegisterAssembliesControllers(IServiceRegistry container)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var controllerTypes = assembly.GetTypes().Where(t => !t.IsAbstract && (typeof(IController).IsAssignableFrom(t) || typeof(IHttpController).IsAssignableFrom(t)));

                foreach (var controllerType in controllerTypes)
                {
                    container.Register(controllerType, new PerRequestLifeTime());
                }
            }
        }
    }
}