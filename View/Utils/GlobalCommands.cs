using System.Windows.Input;

namespace HuTaoHelper.View.Utils; 

/// <summary>
/// Global command definitions to help using events inside data templates
/// </summary>
public static class GlobalCommands {
	public static readonly RoutedCommand CheckIn = new("CheckIn", typeof(GlobalCommands));
	public static readonly RoutedCommand AutoLogin = new("AutoLogin", typeof(GlobalCommands));
}