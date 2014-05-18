namespace Shuttle.ESB.SqlServer
{
	public interface ISqlServerConfiguration
	{
		string SubscriptionManagerConnectionStringName { get; }
		string ScriptFolder { get; }
	}
}