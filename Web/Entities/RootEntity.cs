using System;
using Newtonsoft.Json;

namespace HuTaoHelper.Web.Entities {
	/// <summary>
	/// Entity for recieving data from api
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RootEntity<T> {
		[JsonProperty("retcode")] public int Retcode { get; set; }
		[JsonProperty("message")] public string Message { get; set; } = null!;
		[JsonProperty("data")] public T Data { get; set; } = Activator.CreateInstance<T>();

		public RootEntity<T> ValidateResponseCode() {
			return Retcode switch {
				0 => this,
				-5003 => throw new Exception($"Error -5003: {Message}"),
				_ => throw new Exception($"Unknown error {Retcode}: {Message}")
			};
		}

		public override string ToString() {
			return $"Response code: {Retcode}";
		}
	}
}