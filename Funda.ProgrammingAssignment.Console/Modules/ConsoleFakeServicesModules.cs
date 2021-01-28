using Autofac;
using Funda.ProgrammingAssignment.Domain.Services;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    //This module just loads a fake repository that simulates the result (and shows the fact that, using a repository interface, we mask the complexity of how we retrieve this data)
    public class ConsoleFakeServicesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FakePropertiesRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}