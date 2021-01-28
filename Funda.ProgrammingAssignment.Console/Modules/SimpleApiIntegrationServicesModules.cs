using Autofac;
using Funda.ProgrammingAssignment.ServiceProxy;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    //This module injects a simple,single threaded implementation of the repository that is going to ask to the Funda API the data
    public class SimpleApiIntegrationServicesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FundaApiBasedRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}