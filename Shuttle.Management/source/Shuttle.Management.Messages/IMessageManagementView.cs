using System.Collections.Generic;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    public interface IMessageManagementView
    {
        string SourceQueueUriValue { get; set; }
        string DestinationQueueUriValue { get; set; }
        int FetchCountValue { get; set; }
        bool HasSelectedMessages { get; }
        IEnumerable<TransportMessage> SelectedMessages { get; }
        void PopulateQueues(IEnumerable<Queue> queues);
        void AddFetchCount(int count);
        void PopulateMessages(IEnumerable<TransportMessage> messages);
        void ClearMessages();
        TransportMessage SelectedTransportMessage();
    	void ShowMessage(TransportMessage transportMessage, object message);
    	void ClearMessageView();
    	void CheckAll();
    	void InvertChecks();
    }
}