namespace HuTaoHelper.Core.Core; 

public class ApiCookies {
	public Dictionary<string, string> Values { get; set; } = new();

	/// <summary>
	/// Check if user cookies are valid for api
	/// </summary>
	/// <returns></returns>
	public bool IsValid() {
		return Constants.ApiRequiredCookies.All(required => Values.ContainsKey(required)
		                                       && !string.IsNullOrWhiteSpace(Values[required]));
	}
	
	public string ToCookie() {
		return string.Join("; ", Values.Select(pair => $"{pair.Key}={pair.Value}"));
	}

	public bool ParseFrom(Dictionary<string, string> cookies) {
		Values.Clear();

		foreach (var required in Constants.ApiRequiredCookies) {
			if (!cookies.ContainsKey(required)) {
				return false;
			} else {
				Values[required] = cookies[required];
			}
		}

		return true;
	}
}