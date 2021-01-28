using System.Collections.Generic;
using ConsoleTableExt;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper
{
    //This "Dumper" will write to console a pretty printed table
    public class TableRealEstateAgentSalesDataConsoleDumper : BaseRealEstateAgentSalesDataConsoleDumper
    {
        private readonly IRealEstateAgentSalesDataTableCreator _dataTableCreator;

        public TableRealEstateAgentSalesDataConsoleDumper(IRealEstateAgentSalesDataTableCreator dataTableCreator)
        {
            _dataTableCreator = dataTableCreator;
        }

        protected override void DumpToConsoleUsingSpecificDumper(IEnumerable<RealEstateAgentSalesDto> agentSales)
        {
            ConsoleTableBuilder
                .From(_dataTableCreator.CreateDataTable(agentSales))
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }
    }
}