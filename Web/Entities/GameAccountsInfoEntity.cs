using System.Collections.Generic;
using Newtonsoft.Json;

namespace HuTaoHelper.Web.Entities; 

/// <summary>
/// Game accounts info
/// </summary>
public class GameAccountsInfoEntity : RootEntity<GameAccountsInfoData> {
}

public class GameAccountsInfoData {
	[JsonProperty("list")] public List<GameAccount> Accounts { get; set; } = null!;
}

public class GameAccount {
	[JsonProperty("has_role")] public bool HasRole { get; set; }
	[JsonProperty("game_id")] public int IdOfGame { get; set; }
	[JsonProperty("game_role_id")] public string AccountUid { get; set; } = null!;
	[JsonProperty("nickname")] public string Nickname { get; set; } = null!;
	[JsonProperty("region")] public string Region { get; set; } = null!;
	[JsonProperty("region_name")] public string RegionName { get; set; } = null!;
	[JsonProperty("level")] public int Level { get; set; }
	[JsonProperty("background_image")] public string BackgroundImage { get; set; } = null!;
	[JsonProperty("is_public")] public bool IsPublic { get; set; }
}