using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HuTaoHelper.Core; 

/// <summary>
/// All constants. Strings are packed to prevent direct github search for them, sorry
/// </summary>
[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")]
public static class Constants {
	public static string GameName = "R2Vuc2hpbiBJbXBhY3Q=";
	public static string GameProcessName = "R2Vuc2hpbkltcGFjdA==";
	public static string AuthenticationBrowserSource = "aHR0cHM6Ly9ob3lvbGFiLmNvbS9ob21l";

	#region WebApi

	public static string ApiActId = "ZTIwMjEwMjI1MTkzMTQ4MQ==";
	public static string ApiOrigin = "aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20=";
	public static string ApiSignDay = "aHR0cHM6Ly9zZy1oazRlLWFwaS5ob3lvbGFiLmNvbS9ldmVudC9zb2wvaW5mbz9hY3RfaWQ9";
	public static string ApiSignDayRef = "aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20veXMvZXZlbnQvc2lnbmluLXNlYS12My9pbmRleC5odG1sP2FjdF9pZD0=";
	public static string ApiHome = "aHR0cHM6Ly9zZy1oazRlLWFwaS5ob3lvbGFiLmNvbS9ldmVudC9zb2wvaG9tZT9sYW5nPXJ1LXJ1JmFjdF9pZD0=";
	public static string ApiHomeRef = "aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20v";
	public static string ApiSign = "aHR0cHM6Ly9zZy1oazRlLWFwaS5ob3lvbGFiLmNvbS9ldmVudC9zb2wvc2lnbg==";
	public static string ApiSignRef = "aHR0cHM6Ly9hY3QuaG95b2xhYi5jb20veXMvZXZlbnQvc2lnbmluLXNlYS12My9pbmRleC5odG1sP2FjdF9pZD0=";
	
	public static readonly string[] ApiRequiredCookies = {
		"X01IWVVVSUQ=", "REVWSUNFRlBfU0VFRF9JRA==", "REVWSUNFRlBfU0VFRF9USU1F",
		"REVWSUNFRlA=", "bWkxOG5MYW5n", "bHRva2Vu", "bHR1aWQ=", "Y29va2llX3Rva2Vu", "YWNjb3VudF9pZA=="
	};

	#endregion

	private static string Extract(string source) {
		return Encoding.UTF8.GetString(Convert.FromBase64String(source));
	}
	
	public static void Load() {
		GameName = Extract(GameName);
		GameProcessName = Extract(GameProcessName);
		AuthenticationBrowserSource = Extract(AuthenticationBrowserSource);
		
		ApiActId = Extract(ApiActId);
		ApiOrigin = Extract(ApiOrigin);
		ApiSignDay = Extract(ApiSignDay);
		ApiSignDayRef = Extract(ApiSignDayRef);
		ApiHome = Extract(ApiHome);
		ApiHomeRef = Extract(ApiHomeRef);
		ApiSign = Extract(ApiSign);
		ApiSignRef = Extract(ApiSignRef);
		
		for (var i = 0; i < ApiRequiredCookies.Length; i++) {
			ApiRequiredCookies[i] = Extract(ApiRequiredCookies[i]);
		}
	}
}