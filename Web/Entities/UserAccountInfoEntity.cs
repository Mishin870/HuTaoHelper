using Newtonsoft.Json;

namespace HuTaoHelper.Web.Entities; 

/// <summary>
/// Web user account info
/// </summary>
public class UserAccountInfoEntity : RootEntity<UserAccountInfoData> {
}

public class UserAccountInfoData {
	[JsonProperty("mobile")] public string Mobild { get; set; } = null!;
	[JsonProperty("account_id")] public string AccountId { get; set; } = null!;
	[JsonProperty("account_name")] public string AccountName { get; set; } = null!;
	[JsonProperty("email")] public string Email { get; set; } = null!;
	[JsonProperty("area_code")] public string AreaCode { get; set; } = null!;
	[JsonProperty("facebook_name")] public string FacebookName { get; set; } = null!;
	[JsonProperty("twitter_name")] public string TwitterName { get; set; } = null!;
	[JsonProperty("game_center_name")] public string GameCenterName { get; set; } = null!;
	[JsonProperty("google_name")] public string GoogleName { get; set; } = null!;
	[JsonProperty("apple_name")] public string AppleName { get; set; } = null!;
	[JsonProperty("nick_name")] public string NickName { get; set; } = null!;
	[JsonProperty("sony_name")] public string SonyName { set; get; } = null!;
	[JsonProperty("steam_name")] public string SteamName { set; get; } = null!;
	[JsonProperty("user_icon_id")] public int UserIconId { get; set; }
	[JsonProperty("gender")] public int Gender { get; set; }
}