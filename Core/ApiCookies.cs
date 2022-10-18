using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace HuTaoHelper.Core; 

public class ApiCookies {
	private static readonly string[] RequiredCookies = {
		"_MHYUUID", "DEVICEFP_SEED_ID", "DEVICEFP_SEED_TIME", "DEVICEFP", "mi18nLang", 
		"ltoken", "ltuid", "cookie_token", "account_id"
	};
	
	public Dictionary<string, string> Values { get; set; } = new();

	/// <summary>
	/// Check if user cookies are valid for api
	/// </summary>
	/// <returns></returns>
	public bool IsValid() {
		return RequiredCookies.All(required => Values.ContainsKey(required)
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

		foreach (var required in RequiredCookies) {
			if (!remapped.ContainsKey(required)) {
				return false;
			} else {
				Values[required] = remapped[required];
			}
		}

		return true;
	}
}