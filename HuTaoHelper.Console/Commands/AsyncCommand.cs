namespace HuTaoHelper.Console.Commands; 

/// <summary>
/// Special command version for easy async operations handling
/// </summary>
public abstract class AsyncCommand : ICommand {
	public void Execute(List<string> args) {
		Task.Run(() => ExecuteAsync(args)).Wait();
	}

	/// <summary>
	/// Need to override this in order to execute async operations
	/// </summary>
	/// <param name="args">Command line arguments</param>
	/// <returns></returns>
	protected abstract Task ExecuteAsync(List<string> args);
	
	public abstract string Help { get; }
}