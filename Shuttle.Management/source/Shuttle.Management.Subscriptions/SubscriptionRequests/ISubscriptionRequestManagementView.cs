using System;
using System.Collections.Generic;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
	public interface ISubscriptionRequestManagementView
	{
		void PopulateSubscriberUris(IEnumerable<string> uris);
		string InboxWorkQueueUriValue { get; }
		string DeclineReasonValue { get; }
		void AddRequest(string messageType, bool declined, string declinedBy, DateTime? declinedDate, string declinedReason);
		List<string> SelectedMessageTypes { get; }
		void InvertChecks();
		void RemoveMessageType(string inboxWorkQueueUri, string messageType);
		void DeclineMessageType(string inboxWorkQueueUri, string messageType, string declinedBy, DateTime declinedDate, string declinedReason);
		void CheckAll();
		string DataStoreValue { get; }
		void PopulateDataStores(IEnumerable<DataStore> list);
		void ClearRequests();
	}
}