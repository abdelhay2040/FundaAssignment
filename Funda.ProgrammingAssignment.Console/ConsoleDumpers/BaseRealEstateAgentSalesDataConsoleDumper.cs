using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers
{
    //The "dumper" is the class used to write to console the results. This is the common class that prints the same header and info. 
    //The printing of the result is delegated to a specific instance of this abstract class
    public abstract class BaseRealEstateAgentSalesDataConsoleDumper : IRealEstatesAgentSalesDataConsoleDumper
    {
        protected abstract void DumpToConsoleUsingSpecificDumper(IEnumerable<RealEstateAgentSalesDto> agentSales);

        public void DumpToConsole(GetTopSellersResult result, int numberOfRequestedResults, bool isFakeApiResult)
        {
            if (result == null || !result.AgentSales.Any())
                System.Console.WriteLine("No available results found for selected criteria.");
            else
            {
                DumpHeader(result, numberOfRequestedResults, isFakeApiResult);
                DumpToConsoleUsingSpecificDumper(result.AgentSales);
            }
        }

        public void PrintHeaderMessage()
        {
            Colorful.Console.WriteLine("Welcome Funda Assignment", Color.DarkGoldenrod);
            Colorful.Console.WriteLine("Trying to fetch all the required data for the request. It could take some time...");
        }

        private static void DumpHeader(GetTopSellersResult result, int numberOfRequestedResults, bool isFakeApiResult)
        {
            Colorful.Console.Write($"Showing first {numberOfRequestedResults} results for the search of: ");
            Colorful.Console.Write($"{string.Join(" - ", result.SearchTerms)}", Color.DarkGoldenrod);
            Colorful.Console.WriteLine();
            if (isFakeApiResult)
            {
                Colorful.Console.WriteLine();
                Colorful.Console.WriteLine("------------ THIS IS A FAKE RESULT GENERATED USING FAKE INTEGRATION API ------------", Color.Crimson);
            }

            Colorful.Console.ResetColor();
            Colorful.Console.WriteLine($"Total Number of processed objects: {result.TotalNumberOfObjectsForRequest}", Color.CornflowerBlue);
            Colorful.Console.WriteLine($"Elapsed Time: {result.ElapsedTime}", Color.CornflowerBlue);
        }
    }
}