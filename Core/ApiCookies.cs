using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.WebView2.Core;

namespace HuTaoHelper.Core; 

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

	public bool ParseFrom(List<CoreWebView2Cookie> cookies) {
		Values.Clear();
		
		var remapped = new Dictionary<string, string>();
		foreach (var cookie in cookies) {
			remapped[cookie.Name] = cookie.Value;
		}

		foreach (var required in Constants.ApiRequiredCookies) {
			if (!remapped.ContainsKey(required)) {
				return false;
			} else {
				Values[required] = remapped[required];
			}
		}

		return true;
	}
}