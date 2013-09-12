using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;

namespace Shuttle.Management.Shell
{
	public class ManagementShellPresenter : IManagementShellPresenter
	{
		private readonly IManagementShellView view;
		private readonly TaskQueue taskQueue;
		private readonly IManagementConfiguration managementConfiguration;
		private readonly List<IManagementModulePresenter> presenters = new List<IManagementModulePresenter>();

		public ManagementShellPresenter(IManagementShellView view)
		{
			this.view = view;

			taskQueue = new TaskQueue();

			ActionAppender.Register(view.LogMessage);

			managementConfiguration = new ManagementConfiguration();

			managementConfiguration.Initialize();
		}

		public void Dispose()
		{
			foreach (var presenter in presenters)
			{
				presenter.AttemptDispose();
			}

			taskQueue.Dispose();
		}

		public void OnViewReady()
		{
			var reflectionService = new ReflectionService();

			var moduleTypes = reflectionService.GetTypes<IManagementModule>();

			foreach (var type in moduleTypes)
			{
				if (!type.HasDefaultConstructor())
				{
					Log.Warning(string.Format(ManagementResources.ManagementModuleInitializerHasNoDefaultConstructor, type.FullName));
				}
				else
				{
				    var module = ((IManagementModule) Activator.CreateInstance(type));

				    module.Configure(managementConfiguration);

                    foreach (ManagementModulePresenter presenter in module.Presenters)
				    {
                        presenter.TaskQueue = taskQueue;
                        presenter.ManagementConfiguration = managementConfiguration;

                        view.AddManagementModulePresenter(presenter);

                        presenters.Add(presenter);
                    }
                }
			}
		}
	}
}