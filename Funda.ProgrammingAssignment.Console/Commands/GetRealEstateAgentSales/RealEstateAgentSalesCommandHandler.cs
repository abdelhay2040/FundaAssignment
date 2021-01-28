using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper;
using Funda.ProgrammingAssignment.Console.Modules;
using Funda.ProgrammingAssignment.Domain.BL;
using Microsoft.Extensions.Configuration;

namespace Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales
{
    //This is the handler of the command (the class that contains the logic executed when invoked the command)
    //PLEASE NOTE: In this scenario, having a simple CommandLine application, the Autofac container was built INSIDE the command Handler. This was done for the sake of simplicity in order to provide a quick way to show the usage of the different Autofac modules depending on the options of the command.
    //However, in a long-lasting application (an application that lives for more than a single command execution then stops) this IS NOT A GOOD DESIGN. The IoC container should always be built once and at the beginning of the lifecycle of the app. Then spawn the different lifetime scopes.
    //If it was a long-lasting application, i would have created a multitenant application (to simulate the different scenarios) or used keyed instances

    class RealEstateAgentSalesCommandHandler
    {
        public static int RunGetRealEstateAgentSales(RealEstateAgentSalesCommandOptions opts, IConfiguration configuration)
        {
            Task.Run(() => GetTopRealEstateAgentSalesAsync(opts, configuration)).Wait();
            return 0;
        }

        private static async Task GetTopRealEstateAgentSalesAsync(RealEstateAgentSalesCommandOptions opts,
            IConfiguration configuration)
        {
            var builder = new ContainerBuilder();


            builder.RegisterModule<ConsoleBaseModules>();
            builder.RegisterInstance(configuration).AsImplementedInterfaces().SingleInstance();
            RegisterServiceModules(opts, builder);

            var container = builder.Build();

            using (var context = container.BeginLifetimeScope())
            {
                var dataDumper = context.Resolve<IRealEstatesAgentSalesDataConsoleDumper>();

                dataDumper.PrintHeaderMessage();
                var result = (await context.Resolve<IRealEstateAgentsBl>().GetTopSellers(opts.SearchTerms, opts.NumberOfResults));

                dataDumper.DumpToConsole(result, opts.NumberOfResults, opts.UseFakeApi);
            }
        }

        private static void RegisterServiceModules(RealEstateAgentSalesCommandOptions opts, ContainerBuilder builder)
        {
            if (opts.UseFakeApi)
                builder.RegisterModule<ConsoleFakeServicesModules>();
            else
            {
                builder.RegisterModule<ConsoleRealServicesModules>();
                builder.RegisterModule<SimpleApiIntegrationServicesModules>();

            }

        }
    }
}
