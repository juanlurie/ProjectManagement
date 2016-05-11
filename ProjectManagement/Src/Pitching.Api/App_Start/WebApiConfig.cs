using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Pitching.Api
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Configure(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Filters.Add(new LoggingFilterAttribute());
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = new TicketDataFormat(new ClearText())
            });

            app.UseCors(CorsOptions.AllowAll);

            config.Routes.MapHttpRoute(
                name: RouteNames.DefaultApi,
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                });

            app.UseWebApi(config);

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            return config;
        }
    }
}