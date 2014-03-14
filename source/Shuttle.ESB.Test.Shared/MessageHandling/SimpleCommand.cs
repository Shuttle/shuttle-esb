namespace Shuttle.ESB.Test.Shared.Mocks
{
    public class SimpleCommand : object
    {
        public SimpleCommand()
            : this(typeof(SimpleCommand).GetType().FullName)
        {
        }

        public SimpleCommand(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
