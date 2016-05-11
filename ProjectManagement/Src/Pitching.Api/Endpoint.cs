using System;
using System.Web;
using Hermes.EntityFramework;
using Hermes.Messaging;
using Hermes.Messaging.EndPoints;
using Hermes.Messaging.Transports.SqlTransport;
using Hermes.Serialization.Json;
using MessageContracts;
using ProjectManagement.Contracts;
using ProjectManagement.Persistence;

namespace Pitching.Api
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
                .UseSqlTransport("SqlTransport")
                .ConfigureEntityFramework<ProjectManagementContext>("ProjectManagement");
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

            var isCommand = typeof(IDomainCommand).IsAssignableFrom(type);

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
