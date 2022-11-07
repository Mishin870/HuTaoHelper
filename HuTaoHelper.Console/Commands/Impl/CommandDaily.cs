using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Web.Tools;

namespace HuTaoHelper.Console.Commands.Impl; 

public class CommandDaily : AsyncCommand {
	protected override async Task ExecuteAsync(List<string> args) {
		var count = Settings.Instance.Accounts.Count;
		var current = 1;
		var random = new Random();
		
		foreach (var (_, account) in Settings.Instance.Accounts) {
			Logging.PostEvent($"Processing \"{account.Name}\" account [{current} / {count}]");
			await DailyCheckIn.DoCheckInAsync(account);

			var seconds = 2 + random.Next(3);
			Logging.PostEvent($"Wait random time to be safe: {seconds} seconds");
			await Task.Delay(TimeSpan.FromSeconds(seconds));

			Logging.PostEvent("");
			current++;
		}
	}

	public override string Help => "- do daily check-in for all accounts";
}