using System.Collections.Generic;

namespace Shuttle.Management.Shell
{
	public interface IManagementModule
	{
		void Configure(IManagementConfiguration managementConfiguration);
	    IEnumerable<IManagementModulePresenter> Presenters { get; }
	}
}