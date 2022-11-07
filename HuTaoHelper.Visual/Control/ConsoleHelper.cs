namespace HuTaoHelper.Visual.Control; 

/// <summary>
/// Utilities for command window
/// </summary>
public static class ConsoleHelper {
	/// <summary>
	/// Detaches the application from the command window
	/// </summary>
	public static void HideConsole() {
		NativeMethods.FreeConsole();
	}
}