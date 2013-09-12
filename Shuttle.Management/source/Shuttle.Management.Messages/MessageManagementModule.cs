using System.Collections.Generic;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    public class MessageManagementModule : IManagementModule
    {
        public void Configure(IManagementConfiguration managementConfiguration)
        {
        }

        public IEnumerable<IManagementModulePresenter> Presenters
        {
            get
            {
                return new List<IManagementModulePresenter>
                           {
                               new MessageManagementPresenter()
                           };
            }
        }
    }
}