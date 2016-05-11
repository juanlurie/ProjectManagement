using System;
using System.Configuration;
using Hermes.Logging;
using Hermes.Messaging.Configuration;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Owin;

namespace MediaManagement.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            InitializeLogging();

            LogFactory.BuildLogger = type => new Log4NetLogger(type);

            var config = WebApiConfig.Configure(app);

            var endpoint = new Endpoint();
            endpoint.Start();

            config.DependencyResolver = ((WebApiAutofacAdapter)Settings.RootContainer).BuildAutofacDependencyResolver();
        }

        private static void InitializeLogging()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var patternLayout = new PatternLayout { ConversionPattern = "%d [%t] %-5p %c %m%n" };

            patternLayout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                Layout = patternLayout,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaxSizeRollBackups = 4,
                MaximumFileSize = "1024KB",
                StaticLogFileName = true,
                File = ConfigurationManager.AppSettings["LogFileLocation"]
            };

            var emailAppender = new SmtpAppender
            {
                Layout = patternLayout,
                SmtpHost = "relay.clientele.local",
                From = "noreply@clientele.co.za",
                Subject = ConfigurationManager.AppSettings["Octopus.Environment.Name"] + "Media Management Api Exception",
                To = GetEmailAddressForEnvironment(),
                Evaluator = new LevelEvaluator(Level.Fatal),
                BufferSize = 1,
                Lossy = true
            };

            appender.ActivateOptions();
            emailAppender.ActivateOptions();
            hierarchy.Root.AddAppender(appender);
            hierarchy.Root.AddAppender(emailAppender);

            hierarchy.Root.Level = Level.Warn;
            hierarchy.Configured = true;
        }

        private static string GetEmailAddressForEnvironment()
        {
            try
            {
                var emailAddress = ConfigurationManager.AppSettings["DeveloperGroup"];
                if (emailAddress == null || emailAddress.Trim().Length < 1)
                {
                    throw new Exception("DeveloperGroup variable not set");
                }
                return emailAddress;
            }
            catch (Exception)
            {

                return "Unknown Variable ";
            }
        }
    }
}