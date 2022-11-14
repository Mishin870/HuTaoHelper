using System.Diagnostics.CodeAnalysis;
using System.Text;
using HuTaoHelper.Core.Core.Migrations;
using HuTaoHelper.Notifications.Registry;
using HuTaoHelper.Notifications.Target;
using Newtonsoft.Json;

namespace HuTaoHelper.Core.Core; 

/// <summary>
/// App settings including user accounts
/// </summary>
[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
public class Settings {
	public static Settings Instance { get; set; } = null!;
	private const string FILE_NAME = "settings.json";

	public AutologinSettings Autologin = new();
	public Dictionary<int, Account> Accounts = new();

	/// <summary>
	/// Current selected app language
	/// </summary>
	public string Language = "";
	
	/// <summary>
	/// Current free id for new accounts
	/// </summary>
	public int IdCounter;

	/// <summary>
	/// Configured notification targets
	/// </summary>
	public Dictionary<string, INotificationTarget> Notifications = new();

	/// <summary>
	/// Settings version for migrations
	/// </summary>
	public long Version = Constants.SettingsVersion;

	private static string MakeFilePath() {
		return Path.Join(Directory.GetCurrentDirectory(), FILE_NAME);
	}
	
	/// <summary>
	/// Save current settings <c>Instance</c> into a file
	/// </summary>
	public static void Save() {
		File.WriteAllText(MakeFilePath(),
			JsonConvert.SerializeObject(Instance, 
				new NotificationJsonConverter()), 
			Encoding.UTF8);
		ViewCallbacks.CallbackRefreshAccountsList();
	}

	/// <summary>
	/// Load application settings into current <c>Instance</c>
	/// </summary>
	public static void Load() {
		var path = MakeFilePath();

		if (File.Exists(path)) {
			var content = File.ReadAllText(path, Encoding.UTF8);

			var migrator = new SettingsMigrator(content);
			migrator.Run();
			content = migrator.GetResult();
			
			var settings = JsonConvert.DeserializeObject<Settings>(content, new NotificationJsonConverter());

			if (settings != null) {
				Instance = settings;
				ViewCallbacks.CallbackRefreshAccountsList();
				return;
			}
		}

		Instance = new Settings();
		ViewCallbacks.CallbackRefreshAccountsList();
	}

	/// <summary>
	/// Create a new account and automatically register in the database
	/// </summary>
	/// <param name="login"></param>
	/// <param name="password"></param>
	/// <returns>Created account</returns>
	public Account CreateAccount(string login, string password) {
		var account = new Account {
			Login = login,
			Password = password,
			Id = IdCounter
		};
		IdCounter++;
		Accounts[account.Id] = account;

		return account;
	}

	/// <summary>
	/// Unregister account and remove it
	/// </summary>
	/// <param name="account">Account to remove</param>
	public void RemoveAccount(Account account) {
		Accounts.Remove(account.Id);
	}

	/// <summary>
	/// Register notification target in settings
	/// </summary>
	/// <param name="code">Code of target</param>
	/// <param name="target"></param>
	public void AddNotificationTarget(string code, INotificationTarget target) {
		Notifications[code] = target;
	}
	
	/// <summary>
	/// Get registered notification target by code or null
	/// </summary>
	/// <param name="code"></param>
	/// <returns></returns>
	public INotificationTarget? GetNotificationTarget(string code) {
		return Notifications.GetValueOrDefault(code);
	}

	/// <summary>
	/// Remove registered notification target by code
	/// </summary>
	/// <param name="code"></param>
	public void RemoveNotificationTarget(string code) {
		Notifications.Remove(code);
	}
}