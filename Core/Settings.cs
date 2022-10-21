using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using HuTaoHelper.Core.Migrations;
using HuTaoHelper.View.Utils;
using Newtonsoft.Json;

namespace HuTaoHelper.Core; 

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
	/// Current free id for new accounts
	/// </summary>
	public int IdCounter;
	
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
		File.WriteAllText(MakeFilePath(), JsonConvert.SerializeObject(Instance), Encoding.UTF8);
		ViewUtils.CallbackRefreshAccountsList();
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
			
			var settings = JsonConvert.DeserializeObject<Settings>(content);

			if (settings != null) {
				Instance = settings;
				ViewUtils.CallbackRefreshAccountsList();
				return;
			}
		}

		Instance = new Settings();
		ViewUtils.CallbackRefreshAccountsList();
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
}