using System.Transactions;
using RabbitMQ.Client;

namespace Shuttle.ESB.RabbitMq
{
	sealed class RabbitMqResourceManager : IEnlistmentNotification
	{
		private readonly IModel _channel;

		public RabbitMqResourceManager(IModel channel, Transaction transaction)
		{
			_channel = channel;
			_channel.TxSelect();
			transaction.EnlistVolatile(this, EnlistmentOptions.None);
		}

		public RabbitMqResourceManager(IModel channel)
		{
			_channel = channel;
			_channel.TxSelect();

			if (Transaction.Current != null)
			{
				Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
			}
		}

		public void Commit(Enlistment enlistment)
		{
			_channel.TxCommit();
			enlistment.Done();
		}

		public void InDoubt(Enlistment enlistment)
		{			
			Rollback(enlistment);
		}

		public void Prepare(PreparingEnlistment preparingEnlistment)
		{
			preparingEnlistment.Prepared();
		}

		public void Rollback(Enlistment enlistment)
		{
			_channel.TxRollback();
			enlistment.Done();
		}
	}
}