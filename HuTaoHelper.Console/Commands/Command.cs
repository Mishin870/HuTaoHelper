namespace HuTaoHelper.Console.Commands; 

/// <summary>
/// Any executable command for HuTaoHelper.Console
/// </summary>
public interface ICommand {
	/// <summary>
	/// Run command with arguments
	/// </summary>
	/// <param name="args">Command line arguments</param>
	void Execute(List<string> args);
	
	/// <summary>
	/// Get help on command for displaying if something goes wrong
	/// </summary>
	string Help { get; }
}