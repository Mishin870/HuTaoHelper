namespace HuTaoHelper.Core.Core; 

/// <summary>
/// Core module utility class for GUI callbacks\
/// Will do nothing if the application is running without a GUI
/// </summary>
public static class ViewCallbacks {
	/// <summary>
	/// GUI callback for refreshing accounts list
	/// </summary>
	public static Action CallbackRefreshAccountsList = () => { };
}