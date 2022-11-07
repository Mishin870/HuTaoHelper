using Newtonsoft.Json;

namespace HuTaoHelper.Core.Web.Client; 

/// <summary>
/// Web api simple wrapper
/// </summary>
public class ApiClient {
	private string Cookie { get; }

	private HttpClient Client { get; }

	public ApiClient(string cookie) {
		if (string.IsNullOrEmpty(cookie)) {
			throw new ArgumentException("Cookie is null or empty");
		}

		Cookie = cookie;
		Client = new HttpClient(new HttpClientHandler { UseCookies = false });
	}

	public async Task<T> GetRequestAsync<T>(string path, Action<ApiHttpRequestMessage>? postProcess = null) {
		var req = new Uri($"{path}");
		return await ExecuteRequestAsync<T>(req, HttpMethod.Get, null, postProcess);
	}

	public async Task<T> PostRequestAsync<T>(string path, JsonContent? jsonContent = null,
		Action<ApiHttpRequestMessage>? postProcess = null) {
		var req = new Uri($"{path}");
		return await ExecuteRequestAsync<T>(req, HttpMethod.Post, jsonContent, postProcess);
	}

	private async Task<T> ExecuteRequestAsync<T>(Uri uri, HttpMethod method, HttpContent? content = null, 
		Action<ApiHttpRequestMessage>? postProcess = null) {
		using var requestMessage = BuildHttpRequestMessage(uri, method, content, postProcess);

		var response = await Client.SendAsync(requestMessage);

		var rawResult = await response.Content.ReadAsStringAsync();

		var result = JsonConvert.DeserializeObject<T>(rawResult);
		if (result == null) throw new NullReferenceException("Can't deserealize response");

		return result;
	}

	private HttpRequestMessage BuildHttpRequestMessage(Uri uri, HttpMethod method, HttpContent? content = null,
		Action<ApiHttpRequestMessage>? postProcess = null) {
		var requestMessage = new ApiHttpRequestMessage(method, uri, Cookie);

		if (content != null) {
			requestMessage.Content = content;
		}
			
		postProcess?.Invoke(requestMessage);

		return requestMessage;
	}
}