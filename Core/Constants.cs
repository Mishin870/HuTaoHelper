using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HuTaoHelper.Core; 

/// <summary>
/// All constants. Strings are packed to prevent direct github search for them, sorry
/// </summary>
[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")]
public static class Constants {
	public static readonly string GameName = Extract("R2Vuc2hpbiBJbXBhY3Q=");
	public static readonly string GameProcessName = Extract("R2Vuc2hpbkltcGFjdA==");
	public static readonly string AuthenticationBrowserSource = Extract("aHR0cHM6Ly9ob3lvbGFiLmNvbS9ob21l");

	#region WebApi

	public static readonly string ApiActId = Extract("ZTIwMjEwMjI1MTkzMTQ4MQ==");
	public static readonly string ApiOrigin = Extract("aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20=");
	public static readonly string ApiSignDay = Extract("aHR0cHM6Ly9zZy1oazRlLWFwaS5ob3lvbGFiLmNvbS9ldmVudC9zb2wvaW5mbz9hY3RfaWQ9");
	public static readonly string ApiSignDayRef = Extract("aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20veXMvZXZlbnQvc2lnbmluLXNlYS12My9pbmRleC5odG1sP2FjdF9pZD0=");
	public static readonly string ApiHome = Extract("aHR0cHM6Ly9zZy1oazRlLWFwaS5ob3lvbGFiLmNvbS9ldmVudC9zb2wvaG9tZT9sYW5nPXJ1LXJ1JmFjdF9pZD0=");
	public static readonly string ApiHomeRef = Extract("aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20v");
	public static readonly string ApiSign = Extract("aHR0cHM6Ly9zZy1oazRlLWFwaS5ob3lvbGFiLmNvbS9ldmVudC9zb2wvc2lnbg==");
	public static readonly string ApiSignRef = Extract("aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20veXMvZXZlbnQvc2lnbmluLXNlYS12My9pbmRleC5odG1sP2FjdF9pZD0=");
	public static readonly string ApiUserAccount = Extract("aHR0cHM6Ly9iYnMtYXBpLW9zLmhveW9sYWIuY29tL2NvbW11bml0eS91c2VyL3dhcGkvZ2V0VXNlckZ1bGxJbmZvP2dpZD0y");
	public static readonly string ApiUserAccountRef = Extract("aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20v");
	public static readonly string ApiGameAccounts = Extract("aHR0cHM6Ly9iYnMtYXBpLW9zLmhveW9sYWIuY29tL2dhbWVfcmVjb3JkL2NhcmQvd2FwaS9nZXRHYW1lUmVjb3JkQ2FyZD91aWQ9");
	public static readonly string ApiGameAccountsRef = Extract("aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20v");

	public static readonly string[] ApiRequiredCookies = {
		Extract("X01IWVVVSUQ="), Extract("REVWSUNFRlBfU0VFRF9JRA=="),
		Extract("REVWSUNFRlBfU0VFRF9USU1F"), Extract("REVWSUNFRlA="),
		Extract("bWkxOG5MYW5n"), Extract("bHRva2Vu"), Extract("bHR1aWQ="),
		Extract("Y29va2llX3Rva2Vu"), Extract("YWNjb3VudF9pZA==")
	};

	#endregion

	private static string Extract(string source) {
		return Encoding.UTF8.GetString(Convert.FromBase64String(source));
	}
}