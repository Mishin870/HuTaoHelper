using HuTaoHelper.Console.Commands.Impl;

namespace HuTaoHelper.Console.Commands;

public static class CommandsRegistry {
	private static readonly Dictionary<string, ICommand> Commands = new() {
		{ "help", new CommandHelp() },
		{ "daily", new CommandDaily() },
	};

	public static ICommand? Get(string name) {
		return Commands.GetValueOrDefault(name.ToLowerInvariant());
	}

	public static void PrintHelp(string? forCommand = null) {
		System.Console.WriteLine(@"All commands:");
		System.Console.WriteLine();

		foreach (var (commandName, command) in Commands) {
			if (forCommand != null && commandName != forCommand) continue;
			
			System.Console.WriteLine(@$"{commandName} {command.Help}");
		}
	}
}