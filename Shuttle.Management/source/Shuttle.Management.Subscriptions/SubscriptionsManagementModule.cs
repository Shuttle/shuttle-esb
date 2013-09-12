using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Data;
using Shuttle.Core.Data.Castle;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Castle;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
    public class SubscriptionsManagementModule : IManagementModule
    {
        private readonly WindsorContainer container = new WindsorContainer();

        public void Configure(IManagementConfiguration managementConfiguration)
        {
            if (!managementConfiguration.HasDataStoreRepository)
            {
                Log.Warning(string.Format(ManagementResources.DataStoreRepositoryRequired,
                                          "Shuttle.Management.Subscriptions"));
            }

            container.Register(Component.For<IReflectionService>()
                                   .ImplementedBy<ReflectionService>());

            container.Register(Component.For<IDatabaseConnectionCache>()
                                   .ImplementedBy<ThreadStaticDatabaseConnectionCache>());

            container.Register(Component.For<IDbConnectionConfiguration>()
                                   .ImplementedBy<DbConnectionConfiguration>());

            container.Register(Component.For<IDbConnectionConfigurationProvider>()
                                   .ImplementedBy<ManagementDbConnectionConfigurationProvider>());

            container.RegisterDataAccessCore();

            ((ManagementDbConnectionConfigurationProvider) container.Resolve<IDbConnectionConfigurationProvider>())
                .AddProvider(new DataStoreDbConnectionConfigurationProvider(managementConfiguration));

            container.RegisterDataAccess("Shuttle.Management.Subscriptions");

            container.RegisterSingleton<ISubscriptionRequestManagementPresenter, SubscriptionRequestManagementPresenter>();
            container.RegisterSingleton<ISubscriptionManagementPresenter, SubscriptionManagementPresenter>();
        }

        public IEnumerable<IManagementModulePresenter> Presenters
        {
            get { return container.ResolveAssignable<IManagementModulePresenter>(); }
        }
    }
}