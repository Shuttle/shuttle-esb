namespace Shuttle.ESB.SqlServer
{
    public interface IScriptProvider
    {
        string ScriptBatchSeparator { get; }
        string ScriptFolder { get; }
        string GetScript(Script script, params string[] parameters);
    }
}