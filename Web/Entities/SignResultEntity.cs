using Newtonsoft.Json;

namespace HuTaoHelper.Web.Entities; 

/// <summary>
/// Result status of claiming rewards from the daily check-in
/// </summary>
public class SignResultEntity : RootEntity<SignResultData> {
}

public class SignResultData {
	[JsonProperty("code")] public string Code { get; set; } = null!;
	[JsonProperty("risk_code")] public int RiskCode { get; set; }
}