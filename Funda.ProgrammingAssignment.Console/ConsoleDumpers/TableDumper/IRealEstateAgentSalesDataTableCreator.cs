using System.Collections.Generic;
using System.Data;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper
{
    public interface IRealEstateAgentSalesDataTableCreator
    {
        DataTable CreateDataTable(IEnumerable<RealEstateAgentSalesDto> agentSales);
    }
}