using System;

namespace Funda.ProgrammingAssignment.ServiceProxy.Model.FundaApi
{
    public class ObjectInfo
    {
        public Guid Id { get; set; }
        public long GlobalId { get; set; }
        public long MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }
        public string Woonplaats { get; set; }
        public string Adres { get; set; }
    }
}