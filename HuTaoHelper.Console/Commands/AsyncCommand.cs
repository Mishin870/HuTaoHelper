namespace HuTaoHelper.Console.Commands; 

public abstract class AsyncCommand : ICommand {
	public void Execute(List<string> args) {
		Task.Run(() => ExecuteAsync(args)).Wait();
	}

	protected abstract Task ExecuteAsync(List<string> args);
	
	public abstract string Help { get; }
}