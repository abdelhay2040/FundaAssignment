using System;
using System.IO;
using CommandLine;
using Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales;
using Microsoft.Extensions.Configuration;

namespace Funda.ProgrammingAssignment.Console
{
    //This is a simple Command Line Application. I've used the CommandLine NuGet library in order to speed-up the development process.
    //Actually there's only one Verb (command) and it can be found in the Commands folder (with the associated options)
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var configuration = ParseJsonConfiguration();
                var res = InitCommands(args, configuration);
                return res;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex);
                return -1;
            }
        }

        private static int InitCommands(string[] args, IConfigurationRoot configuration)
        {
            var res = Parser.Default.ParseArguments<RealEstateAgentSalesCommandOptions>(args)
                .MapResult(options => RealEstateAgentSalesCommandHandler.RunGetRealEstateAgentSales(options, configuration),
                    errs => 1);
            return res;
        }

        private static IConfigurationRoot ParseJsonConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }
    }
}
