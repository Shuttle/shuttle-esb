namespace Shuttle.ESB.Core
{
    public interface IRegisterObserverAnd
    {
        IRegisterObserverAnd AndObserver(IObserver observer);
    }
}