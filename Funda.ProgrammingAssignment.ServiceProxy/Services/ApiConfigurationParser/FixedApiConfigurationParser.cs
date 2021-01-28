using Funda.ProgrammingAssignment.ServiceProxy.Model;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser
{
    public class FixedApiConfigurationParser : IApiConfigurationParser
    {
        public const string DefaultBaseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/";
        public const string DefaultKey = "ac1b0b1572524640a0ecc54de453ea9f";
  

        public BasicApiConfiguration GetBasicConfiguration()
        {
            return new BasicApiConfiguration
            {
                BaseUrl = DefaultBaseUrl,
                Key = DefaultKey
            };
        }

    }
}