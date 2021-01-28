namespace Funda.ProgrammingAssignment.Domain.Common.Exceptions
{
    public class ConfigurationException : System.Exception
    {
        private string SectionName { get; set; }

        public ConfigurationException()
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }

        public ConfigurationException(string sectionName, System.Exception inner)
            : base($"Exception raised while reading config file - Section: {sectionName} - See inner exception for further details.", inner)
        {
            SectionName = sectionName;
        }

        public ConfigurationException(string sectionName, string message)
            : base($"Exception raised while reading config file - Section: {sectionName} - {message}")
        {
            SectionName = sectionName;
        }
    }
}