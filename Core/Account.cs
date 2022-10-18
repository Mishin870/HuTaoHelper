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
	public string? Name { get; set; }
	/// <summary>
	/// User status from game
	/// </summary>
	public string? Status { get; set; }

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
}