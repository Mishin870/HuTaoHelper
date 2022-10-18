using System.Windows.Input;

namespace HuTaoHelper.Control; 

/// <summary>
/// Global command definitions to help using events inside data templates
/// </summary>
public static class GlobalCommands {
	public static readonly RoutedCommand CheckIn = new("Check-In", typeof(GlobalCommands));
}