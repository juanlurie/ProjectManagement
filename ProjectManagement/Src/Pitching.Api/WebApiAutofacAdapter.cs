using System;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Builder;
using Autofac.Integration.WebApi;
using Hermes.Ioc;
using Hermes.ObjectBuilder.Autofac;
using IContainer = Autofac.IContainer;

namespace MediaManagement.Api
{
    public class WebApiAutofacAdapter : AutofacAdapter
    {
        public WebApiAutofacAdapter()
            : base(ConfigureApiDependencies())
        {

        }

        private static IContainer ConfigureApiDependencies()
        {
            var builder = new ContainerBuilder();
            var tasksWebApi = Assembly.Load(new AssemblyName("Workflow.HumanTasks.Web.Api"));

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly(), tasksWebApi);
            builder.RegisterWebApiModelBinders(Assembly.GetExecutingAssembly(), tasksWebApi);

            builder.RegisterWebApiModelBinderProvider();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            return builder.Build();
        }

        public AutofacWebApiDependencyResolver BuildAutofacDependencyResolver()
        {
            return new AutofacWebApiDependencyResolver(LifetimeScope);
        }

        public override Hermes.Ioc.IContainer BeginLifetimeScope()
        {
            return new AutofacAdapter(LifetimeScope.BeginLifetimeScope("Hermes"));
        }

        protected override void ConfigureLifetimeScope<T>(DependencyLifecycle dependencyLifecycle, IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration)
        {
            switch (dependencyLifecycle)
            {
                case DependencyLifecycle.SingleInstance:
                    registration.SingleInstance();
                    break;
                case DependencyLifecycle.InstancePerDependency:
                    registration.InstancePerDependency();
                    break;
                case DependencyLifecycle.InstancePerUnitOfWork:
                    registration.InstancePerApiRequest("AutofacWebRequest", "Hermes");
                    break;
                default:
                    throw new ArgumentException("Unknown container lifecycle - " + dependencyLifecycle);
            }
        }
    }
}