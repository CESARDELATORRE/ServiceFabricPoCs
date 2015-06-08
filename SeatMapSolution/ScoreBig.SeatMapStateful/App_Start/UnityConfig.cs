
namespace ScoreBig.SeatMapStateful
{
    using System.Web.Http;
    using Microsoft.Practices.Unity;
    using Microsoft.ServiceFabric.Data;
    using Unity.WebApi;
    using global::ScoreBig.SeatMapStateful.Controllers;

    /// <summary>
    /// Configures dependency injection for Controllers using a Unity container. 
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config, IReliableStateManager stateManager)
        {
            UnityContainer container = new UnityContainer();

            // The default controller needs a state manager to perform operations.
            // Using the DI container, we can inject it as a dependency.
            container.RegisterType<DefaultController>(
                new TransientLifetimeManager(),
                new InjectionConstructor(stateManager));

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}