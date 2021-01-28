using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales
{
    [Verb("topSellingAgents", HelpText = "Print a list of the top N real estate agents that have the most number of properties listed for sale. You can define the search terms and the number of results")]
    internal class RealEstateAgentSalesCommandOptions
    {
        [Option("fa", HelpText = "Set as true to use fake api integrations instead of the real one (testing purposes only)", Default = false, Required = false)]
        public bool UseFakeApi { get; set; }

        [Option("rn", HelpText = "Select the number of results.", Default = 10, Required = false)]
        public int NumberOfResults { get; set; }

        [Option('t', HelpText = "The search terms, separated by a space", Required = true)]
        public IEnumerable<string> SearchTerms { get; set; }


    }
}
