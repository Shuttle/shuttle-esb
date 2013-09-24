namespace Shuttle.ESB.Core
{
	public static class StateKeys
	{
		public const string ActiveState = "ActiveState";
		public const string WorkQueue = "WorkQueue";
		public const string JournalQueue = "JournalQueue";
		public const string ErrorQueue = "ErrorQueue";
		public const string DestinationQueue = "DestinationQueue";
		public const string TransportMessage = "TransportMessage";
		public const string MessageBytes = "MessageBytes";
		public const string TransportMessageStream = "TransportMessageStream";
		public const string Message = "Message";
		public const string MessageHandler = "MessageHandler";
		public const string AvailableWorker = "AvailableWorker";
		public const string MaximumFailureCount = "MaximumFailureCount";
		public const string DurationToIgnoreOnFailure = "DurationToIgnoreOnFailure";
		public const string TransactionComplete = "TransactionComplete";
		public const string TransactionScope = "TransactionScope";
		public const string Working = "Working";
        public const string HasJournalQueue = "HasJournalQueue";
	}
}