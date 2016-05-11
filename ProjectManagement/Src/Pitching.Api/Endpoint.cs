using System;
using System.Web;
using Hermes.EntityFramework;
using Hermes.Messaging;
using Hermes.Messaging.EndPoints;
using Hermes.Messaging.Transports.SqlTransport;
using Hermes.Serialization.Json;
using MediaManagement.Contracts;
using MediaManagement.Contracts.Commands;
using MediaManagement.Persistence;
using MessageContracts;
using Workflow.HumanTasks.ApplicationService.Wireup;
using Workflow.HumanTasks.Contracts.Commands;
using Workflow.HumanTasks.Persistence.Wireup;

namespace MediaManagement.Api
{
    public class Endpoint : LocalEndpoint<WebApiAutofacAdapter>
    {
        protected override void ConfigureEndpoint(IConfigureEndpoint configuration)
        {
            configuration
#if DEBUG
                .DisableDistributedTransactions()
#endif
                .UseJsonSerialization()
                .DefineCommandAs(IsCommand)
                .DefineEventAs(IsEvent)
                .UserNameResolver(GetCurrentUserName)
                .RegisterDependencies<DependencyRegistrar>()
                .RegisterDependencies<QueryServiceDependencyRegistrar>()
                .RegisterDependencies<HumanTasksDependencyRegistrar>()
                .RegisterMessageRoute<ParseSpotList>(new Address("MediaManagement"))
                .UseSqlTransport("SqlTransport")
                .ConfigureEntityFramework<MediaContext>("Media");
        }

        private static string GetCurrentUserName()
        {
            if (HttpContext.Current == null || HttpContext.Current.User == null || string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                return Environment.UserName;

            return HttpContext.Current.User.Identity.Name;
        }

        private static bool IsCommand(Type type)
        {
            if (type == null || type.Namespace == null)
                return false;

            var isCommand = typeof(IWorkflowCommand).IsAssignableFrom(type) || typeof(IDomainCommand).IsAssignableFrom(type);

            return isCommand;
        }

        private static bool IsEvent(Type type)
        {
            if (type == null || type.Namespace == null)
                return false;

            var result = typeof(IEvent).IsAssignableFrom(type) || typeof(IDomainEvent).IsAssignableFrom(type);
            return result;
        }
    }
}
