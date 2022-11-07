namespace HuTaoHelper.Core.Core;

/// <summary>
/// Logging utilities
/// </summary>
public static class Logging {
	public static Action<object?, int> EventsProcessor = (text, durationMs) => {
		Console.WriteLine(@$"{text}");
	};

	/// <summary>
	/// Post a text message to application event log. By default it's a SnackBar at the bottom
	/// </summary>
	/// <param name="text">Text or any object to show (will be converted to string)</param>
	/// <param name="durationMs">Duration of the message in milliseconds</param>
	public static void PostEvent(object? text, int durationMs = 1000) {
		if (text == null) return;

		EventsProcessor(text, durationMs);
	}
}