using HuTaoHelper.Console.Commands.Impl;
using HuTaoHelper.Core.Core;

namespace HuTaoHelper.Console.Commands;

/// <summary>
/// All commands database
/// </summary>
public static class CommandsRegistry {
	private static readonly Dictionary<string, ICommand> Commands = new() {
		{ "help", new CommandHelp() },
		{ "daily", new CommandDaily() },
	};

	/// <summary>
	/// Get command by name or null if it doesn't exist
	/// </summary>
	/// <param name="name">Command name</param>
	/// <returns>Command or null if there is no such command</returns>
	public static ICommand? Get(string name) {
		return Commands.GetValueOrDefault(name.ToLowerInvariant());
	}

	/// <summary>
	/// Print help for selected commands to console
	/// </summary>
	/// <param name="forCommand">Command name filter or null to display all commands</param>
	public static void PrintHelp(string? forCommand = null) {
		Logging.PostEvent(@"All commands:");
		Logging.PostEvent("");

		foreach (var (commandName, command) in Commands) {
			if (forCommand != null && commandName != forCommand) continue;
			
			Logging.PostEvent(@$"{commandName} {command.Help}");
		}
	}
}