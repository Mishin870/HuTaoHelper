using System.Collections.Generic;
using Newtonsoft.Json;

namespace HuTaoHelper.Web.Entities {
	/// <summary>
	/// Current day of the month and all rewards list from the daily check-in
	/// </summary>
	public class HomeEntity : RootEntity<HomeData> {
	}

	public class HomeData {
		[JsonProperty("month")] public int Month { get; set; }
		[JsonProperty("resign")] public bool Resign { get; set; }
		[JsonProperty("now")] public string Now { get; set; } = null!;
		[JsonProperty("awards")] public List<Award> Awards { get; set; } = null!;
	}

	public class Award {
		[JsonProperty("icon")] public string Icon = null!;
		[JsonProperty("name")] public string Name = null!;
		[JsonProperty("cnt")] public int Count;
	}
}