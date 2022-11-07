using Newtonsoft.Json;

namespace HuTaoHelper.Core.Web.Entities; 

/// <summary>
/// Info about status of the daily check-in
/// </summary>
public class SignDayEntity : RootEntity<SignDayData> {
}

public class SignDayData {
	[JsonProperty("total_sign_day")] public int TotalSignDay { get; set; }
	[JsonProperty("today")] public string Today { get; set; } = null!;
	[JsonProperty("is_sign")] public bool IsSign { get; set; }
	[JsonProperty("first_bind")] public bool FirstBind { get; set; }
}