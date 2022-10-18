using Newtonsoft.Json;

namespace HuTaoHelper.Web.Entities; 

/// <summary>
/// Web user account info
/// </summary>
public class UserAccountInfoEntity : RootEntity<UserAccountInfoData> {
}

public class UserAccountInfoData {
	[JsonProperty("user_info")] public UserInfo Info { get; set; } = null!;
}

public class UserInfo {
	[JsonProperty("uid")] public string AccountId { get; set; } = null!;
	[JsonProperty("nickname")] public string Nickname { get; set; } = null!;
	[JsonProperty("introduce")] public string Status { get; set; } = null!;
	[JsonProperty("gender")] public int Gender { get; set; }
	[JsonProperty("avatar_url")] public string AvatarUrl { get; set; } = null!;
}