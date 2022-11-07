namespace HuTaoHelper.Console.Commands.Impl; 

public class CommandHelp : ICommand {
	public void Execute(List<string> args) {
		if (args.Count >= 1) {
			CommandsRegistry.PrintHelp(args[0]);
		} else {
			CommandsRegistry.PrintHelp();			
		}
	}

	public string Help => "[command_name] - get help for commands";
}