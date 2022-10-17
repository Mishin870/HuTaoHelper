using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
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
	public int IdCounter;

	private static string MakeFilePath() {
		return Path.Join(Directory.GetCurrentDirectory(), FILE_NAME);
	}
	
	/// <summary>
	/// Save current settings <c>Instance</c> into a file
	/// </summary>
	public static void Save() {
		File.WriteAllText(MakeFilePath(), JsonConvert.SerializeObject(Instance), Encoding.UTF8);
	}

	/// <summary>
	/// Load application settings into current <c>Instance</c>
	/// </summary>
	public static void Load() {
		var path = MakeFilePath();

		if (File.Exists(path)) {
			var content = File.ReadAllText(path, Encoding.UTF8);
			var settings = JsonConvert.DeserializeObject<Settings>(content);

			if (settings != null) {
				Instance = settings;
				return;
			}
		}

		Instance = new Settings();
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
}