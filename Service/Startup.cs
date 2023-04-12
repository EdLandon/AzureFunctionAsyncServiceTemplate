using Autofac;
using Autofac.Extensions.DependencyInjection.AzureFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerlessLib;
using Service1;
using ServiceLib;
using System;

using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Service1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder
                // This is the required call in order to use autofac in your azure functions app
                .UseAutofacServiceProviderFactory(ConfigureContainer);

            builder.Services
                .AddLogging()
                .AddHttpClient("pollyClient").AddPolicyHandler(GetRetryPolicy());    // Here we are adding "named" http clients.  Because they are used in lots of examples, we may want to consider using <typed> clients. 

        }
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        var log = context.GetLogger();
                        log?.LogInformation($"Request failed with status code {outcome.Result.StatusCode} delaying for {timespan.TotalMilliseconds} milliseconds then making retry {retryAttempt}");
                    }
                );
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            // this is optional and will bind IConfiguration with appsettings.json in
            // the container, like it is usually done in regular dotnet console and
            // web applications.
//            builder.UseAppSettings();

            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            builder.ConfigurationBuilder.AddConfiguration(config);
        }

        private Autofac.IContainer ConfigureContainer(ContainerBuilder builder)
        {
            //builder
            //    .Register(activator =>
            //    {
            //        // Example on how to bind settings from appsettings.json to a class instance
            //        var section = activator.Resolve<IConfiguration>().GetSection(nameof(MySettings));

            //        var instance = section.Get<MySettings>();

            //        // If you expect IConfiguration to change (with reloadOnChange=true), use
            //        // token to rebind.
            //        ChangeToken.OnChange(
            //            () => section.GetReloadToken(),
            //            (state) => section.Bind(state),
            //            instance);

            //        return instance;
            //    })
            //    .AsSelf()
            //    .SingleInstance();


            // Register all functions that resides in a given namespace
            // The function class itself will be created using autofac
            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .InNamespace("Service1.Functions")
                .AsSelf() // Azure Functions core code resolves a function class by itself.
                .InstancePerTriggerRequest(); // This will scope nested dependencies to each function execution
            builder
                .RegisterType<ApplesProcessor>()
                .Named<QueuePayloadModelProcessor>("Event_Domain1_Service1_DoApples1_v1.0");
            builder
                .RegisterType<OrangesProcessor>()
                .Named<QueuePayloadModelProcessor>("Event_Domain1_Service1_DoOranges1_v1.0");

            var container = builder.Build();
            ContainerProvider.Container = container;

            return container;
        }
    }
}
