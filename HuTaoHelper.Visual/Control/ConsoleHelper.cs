namespace HuTaoHelper.Visual.Control; 

public static class ConsoleHelper {
	public static void HideConsole() {
		NativeMethods.FreeConsole();
	}
}