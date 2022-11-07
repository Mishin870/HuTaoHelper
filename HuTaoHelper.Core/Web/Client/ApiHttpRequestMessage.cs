using HuTaoHelper.Core.Core;

namespace HuTaoHelper.Core.Web.Client; 

/// <summary>
/// Our custom http request version to handle headers and cookies
/// </summary>
public class ApiHttpRequestMessage : HttpRequestMessage {
	public ApiHttpRequestMessage(HttpMethod method, Uri uri, string cookie) : base(method, uri) {
		if (string.IsNullOrEmpty(cookie)) {
			throw new ArgumentException("Cookie is null or empty");
		}

		Headers.Add("Accept", "application/json, text/plain, */*");
		Headers.Add("Accept-Language", "en-US,en;q=0.5");
		Headers.Add("Origin", Constants.ApiOrigin);
		Headers.Add("Connection", "keep-alive");
		Headers.Add("Cache-Control", "max-age=0");
		Headers.Add("Cookie", cookie);
	}

	public ApiHttpRequestMessage WithReferer(string referer) {
		Headers.Add("Referer", referer);
		return this;
	}
}