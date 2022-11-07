namespace HuTaoHelper.Console.Commands; 

public interface ICommand {
	void Execute(List<string> args);
	string Help { get; }
}