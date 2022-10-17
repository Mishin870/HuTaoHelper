namespace HuTaoHelper.Core;

/// <summary>
/// User account information
/// </summary>
public class Account {
	/// <summary>
	/// Internal account id for storing
	/// </summary>
	public int Id { get; set; }
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
	/// Data for hoyolab authentication
	/// </summary>
	public string? LToken { get; set; }
	/// <summary>
	/// Data for hoyolab authentication
	/// </summary>
	public string? LTuid { get; set; }

	/// <summary>
	/// Actual name for displaying account in lists
	/// </summary>
	public string DisplayName => Title ?? (Name ?? "");
	/// <summary>
	/// First character from account display name to make fake avatar
	/// </summary>
	public string NameCharacter => DisplayName[..1];
}