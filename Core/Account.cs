using System;
using System.Threading.Tasks;
using HuTaoHelper.Web.Client;
using Newtonsoft.Json;

namespace HuTaoHelper.Core;

/// <summary>
/// User account information
/// </summary>
public class Account {
	/// <summary>
	/// Internal account id for storing
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Game account id
	/// </summary>
	public long Uid { get; set; }
	public string Login { get; set; } = null!;
	public string Password { get; set; } = null!;

	/// <summary>
	/// Custom user title
	/// </summary>
	public string? Title { get; set; }
	/// <summary>
	/// User nickname from game
	/// </summary>
	public string? Name { get; set; } = "NotAuthenticatedUser";
	/// <summary>
	/// User status from game
	/// </summary>
	public string? Status { get; set; }
	/// <summary>
	/// Web user avatar. Default value is a google "no avatar" picture
	/// </summary>
	public string? AvatarUrl { get; set; } =
		"https://lh3.googleusercontent.com/-cXXaVVq8nMM/AAAAAAAAAAI/AAAAAAAAAKI/_Y1WfBiSnRI/photo.jpg?sz=300";

	/// <summary>
	/// Cookies for api authentication
	/// </summary>
	public ApiCookies Cookies { get; set; } = new();
	/// <summary>
	/// User id for telegram bot
	/// </summary>
	public long TelegramId { get; set; }

	/// <summary>
	/// Actual name for displaying account in lists
	/// </summary>
	[JsonIgnore]
	public string DisplayName => Title ?? (Name ?? "");
	/// <summary>
	/// First character from account display name to make fake avatar
	/// </summary>
	[JsonIgnore]
	public string DisplayNameCharacter => DisplayName[..1];

	/// <summary>
	/// Refresh information about game account (first found)
	/// </summary>
	/// <returns>Is refreshing was successful</returns>
	public async Task<bool> RefreshGameInformation() {
		try {
			if (!Cookies.IsValid()) {
				return false;
			}

			var cookie = Cookies.ToCookie();
			var client = new ApiClient(cookie);
			var accountInfo = await client.GetUserAccountInfo();
			var gameAccountsInfo = await client.GetGameAccountsInfo(accountInfo.Data.Info.AccountId);

			foreach (var account in gameAccountsInfo.Data.Accounts) {
				AvatarUrl = accountInfo.Data.Info.AvatarUrl;
				Name = account.Nickname;
				if (long.TryParse(account.AccountUid, out var uid)) {
					Uid = uid;
				}

				Settings.Save();

				return true;
			}

			return false;
		} catch (Exception) {
			return false;
		}
	}
}