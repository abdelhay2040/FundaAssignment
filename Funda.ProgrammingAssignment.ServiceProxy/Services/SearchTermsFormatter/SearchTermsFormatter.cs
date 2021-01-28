using System.Collections.Generic;
using System.Linq;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.SearchTermsFormatter
{
    public class SearchTermsFormatter : ISearchTermsFormatter
    {
        public string Format(IEnumerable<string> searchTerms)
        {
            var resultString = string.Empty;
            if (searchTerms != null && searchTerms.Any())
                resultString = $"/{string.Join("/", searchTerms)}/";

            return resultString;
        }
    }
}