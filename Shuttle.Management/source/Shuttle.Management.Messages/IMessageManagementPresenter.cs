namespace Shuttle.Management.Messages
{
    public interface IMessageManagementPresenter
    {
        void RefreshQueue();
        void Remove();
        void Move();
        void MessageSelected();
        void ReturnToSourceQueue();
        void CheckAll();
        void InvertChecks();
    }
}