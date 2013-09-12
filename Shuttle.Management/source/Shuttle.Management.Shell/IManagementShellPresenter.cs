using System;

namespace Shuttle.Management.Shell
{
    public interface IManagementShellPresenter : IDisposable
    {
        void OnViewReady();
    }
}