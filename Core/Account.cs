namespace HuTaoHelper.Core;

/// <summary>
/// User account information
/// </summary>
public class Account {
	/// <summary>
	/// Account title
	/// </summary>
	public string Name { get; set; } = null!;
	
	public string Login { get; set; } = null!;
	public string Password { get; set; } = null!;
}