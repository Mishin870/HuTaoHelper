using System.Globalization;
using System.Reflection;
using HuTaoHelper.Console.Commands;
using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Localization;

namespace HuTaoHelper.Console; 

public class Program {
	public static void Main(string[] args) {
		Logging.EventsProcessor = (text, _) => {
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (path == null) return;

			var content = $"[{DateTime.Now.ToString("HH:mm:ss")}] {text}\n";
			File.AppendAllText(Path.Combine(path, "HuTaoHelper.Console.log"), content);
		};
		
		if (args.Length >= 1) {
			var commandName = args[0];
			var command = CommandsRegistry.Get(commandName);

			if (command == null) {
				Logging.PostEvent(@$"Command not found: {commandName}");
			} else {
				Settings.Load();
				Translations.Culture = CultureInfo.GetCultureInfo(Settings.Instance.Language);
				command.Execute(args.Skip(1).ToList());
			}
		} else {
			Logging.PostEvent(@"No command. Usage: HuTaoHelper.Console %command%");
			Logging.PostEvent("");
			CommandsRegistry.PrintHelp();
		}
	}
}