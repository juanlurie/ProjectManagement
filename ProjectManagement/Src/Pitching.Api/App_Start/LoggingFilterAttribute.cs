using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Hermes.Logging;

namespace Pitching.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class LoggingFilterAttribute : ActionFilterAttribute
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof (LoggingFilterAttribute));

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            Logger.Info("UpdateAction executing: {0} {1}", actionContext.Request.Method, actionContext.Request.RequestUri);
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            Exception exception = actionExecutedContext.Exception;

            if (exception == null && actionExecutedContext.Response.StatusCode >= HttpStatusCode.InternalServerError)
            {
                exception = new HttpException((int)actionExecutedContext.Response.StatusCode, ResolveMessage(actionExecutedContext));
            }

            if (exception != null)
            {
                var errorMessage = exception.GetFullExceptionMessage();
                Logger.Error("UpdateAction executing: {0} {1} {2}", actionExecutedContext.Request.Method, actionExecutedContext.Request.RequestUri,errorMessage);
            }
            else
            {
                Logger.Info("UpdateAction completed: {0} {1} {2} {3}", 
                            actionExecutedContext.Request.Method, 
                            actionExecutedContext.Request.RequestUri,
                            actionExecutedContext.Response.StatusCode,
                            ResolveMessage(actionExecutedContext));
            }
        }

        private string ResolveMessage(HttpActionExecutedContext actionExecutedContext)
        {
            string reasonPhrase = actionExecutedContext.Response.ReasonPhrase;

            ObjectContent<HttpError> objectContent = actionExecutedContext.Response.Content as ObjectContent<HttpError>;
            
            if (objectContent == null)
                return reasonPhrase;
            
            HttpError httpError = objectContent.Value as HttpError;
            
            if (httpError == null || !httpError.ContainsKey("Message"))
                return reasonPhrase;
            
            string str = httpError["Message"] as string;
            
            return string.IsNullOrWhiteSpace(str) ? reasonPhrase : str;
        }
    }
}