namespace FileBrowser.Commands
{
    public abstract class Command
    {
      public abstract void Execute();
      public bool IsFinished { get; protected set; }
      public bool Condition { get; protected set; }
      public string errorMessage;
    }
}
