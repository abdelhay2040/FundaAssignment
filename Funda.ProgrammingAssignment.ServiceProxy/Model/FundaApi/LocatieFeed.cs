using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Funda.ProgrammingAssignment.ServiceProxy.Model.FundaApi
{
    public class LocatieFeed
    {
        public MetadataInfo Metadata { get; set; }

        public PaginationInfo Paging { get; set; }

        public IEnumerable<ObjectInfo> Objects { get; set; }

        public int TotaalAantalObjecten { get; set; }
    }
}
