using System;
using MaterialDesignThemes.Wpf;

namespace HuTaoHelper.Core;

/// <summary>
/// Logging utilities
/// </summary>
public static class Logging {
	public static readonly SnackbarMessageQueue EVENT_QUEUE = new();

	/// <summary>
	/// Post a text message to application event log. By default it's a SnackBar at the bottom
	/// </summary>
	/// <param name="text">Text to show</param>
	/// <param name="durationMs">Duration of the message in milliseconds</param>
	public static void PostEvent(string text, int durationMs = 1000) {
		EVENT_QUEUE.Enqueue(text, null, null, 
			null, false, false,
			TimeSpan.FromMilliseconds(durationMs));
	}
}