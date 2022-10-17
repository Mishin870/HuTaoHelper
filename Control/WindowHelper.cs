using System.Diagnostics;

namespace HuTaoHelper.Control;

/// <summary>
/// Windows manager
/// </summary>
public static class WindowHelper {
	public static void BringProcessToFront(Process process) {
		var mainWindowHandle = process.MainWindowHandle;
		if (NativeMethods.IsIconic(mainWindowHandle)) {
			NativeMethods.ShowWindow(mainWindowHandle, 9);
		}

		NativeMethods.SetForegroundWindow(mainWindowHandle);
	}
}