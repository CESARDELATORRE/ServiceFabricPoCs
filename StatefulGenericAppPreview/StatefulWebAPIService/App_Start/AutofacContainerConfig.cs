namespace StatefulWebAPIService
{
    using Autofac;
    using Autofac.Integration.WebApi;
    using System;
    using System.Collections.Generic;

    using StatefulWebAPIService.Controllers;
    using System.Reflection;
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Data.Collections;
    using Microsoft.ServiceFabric.Services;



    /// <summary>
    /// Represent the container configuration for DI
    /// </summary>
    public static class AutofacContainerConfig
    {
        static IContainer _container;

        /// <summary>
        /// Get configured container
        /// </summary>
        public static IContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// Configure container
        /// </summary>
        public static void Configure(IReliableStateManager objectManager)
        {
            var builder = new ContainerBuilder();

            //register test type for DI
            builder.RegisterType<TestDI>()
              .As<ITestDI>()
              .InstancePerDependency();

            //register web api controllers
            builder.Register(c => new DefaultController(objectManager)).As<DefaultController>();

            builder.Register(c => new CustomerController(objectManager)).As<CustomerController>();

            //builder.RegisterType<DefaultController>()
            //  .AsSelf()
            //  .WithParameter("objectmanager", objectManager)
            //  .InstancePerDependency();

            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //register unit of work
            //builder.RegisterType<ExpenseUnitOfWork>()
            //  .AsSelf()
            //  .InstancePerLifetimeScope();

            //builder.Register((component) =>
            //{
            //    Func<UrlHelper> func = () =>
            //    {
            //        if (HttpContext.Current != null)
            //        {
            //            var context = new HttpContextWrapper(HttpContext.Current);
            //            var routeData = RouteTable.Routes.GetRouteData(context);

            //            return new UrlHelper(new RequestContext(context, routeData));
            //        }
            //        else
            //            return null;

            //    };

            //    return func;
            //});


            //build the container

            _container = builder.Build();
            
        }
    }
}