using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Data;
using Shuttle.Core.Data.Castle;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Castle;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Scheduling
{
    public class SchedulingManagementModule : IManagementModule
    {
        private readonly WindsorContainer container = new WindsorContainer();

        public void Configure(IManagementConfiguration managementConfiguration)
        {
            if (!managementConfiguration.HasDataStoreRepository)
            {
                Log.Warning(string.Format(ManagementResources.DataStoreRepositoryRequired,
                                          "Shuttle.Management.Scheduling"));
            }

            container.Register(Component.For<IDatabaseConnectionCache>()
                                   .ImplementedBy<ThreadStaticDatabaseConnectionCache>());

            container.Register(Component.For<IDbConnectionConfiguration>()
                                   .ImplementedBy<DbConnectionConfiguration>());

            container.Register(Component.For<IDbConnectionConfigurationProvider>()
                                   .ImplementedBy<ManagementDbConnectionConfigurationProvider>());

            container.RegisterDataAccessCore();

            ((ManagementDbConnectionConfigurationProvider) container.Resolve<IDbConnectionConfigurationProvider>())
                .AddProvider(new DataStoreDbConnectionConfigurationProvider(managementConfiguration));

            container.RegisterDataAccess("Shuttle.Scheduling");

            container.RegisterSingleton<IScheduleManagementPresenter, IScheduleManagementPresenter>();
        }

        public IEnumerable<IManagementModulePresenter> Presenters
        {
            get { return container.ResolveAssignable<IManagementModulePresenter>(); }
        }
    }
}