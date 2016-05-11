using Hermes.Ioc;
using ProjectManagement.Persistence.Queries;

namespace Pitching.Api
{
    public class DependencyRegistrar : IRegisterDependencies
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TestQuery>();
        }
    }
}