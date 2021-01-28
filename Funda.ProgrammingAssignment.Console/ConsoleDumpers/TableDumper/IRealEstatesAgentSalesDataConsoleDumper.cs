using System.Collections.Generic;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper
{
    public interface IRealEstatesAgentSalesDataConsoleDumper
    {
        void DumpToConsole(GetTopSellersResult result, int numberOfRequestedResults, bool isFakeApiResult);
        void PrintHeaderMessage();
    }
}