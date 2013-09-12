using log4net.Core;

namespace Shuttle.Management.Shell
{
    public interface IManagementShellView
    {
        void LogMessage(LoggingEvent loggingEvent);
        void AddManagementModulePresenter(ManagementModulePresenter modulePresenter);
    }
}