using System;
using System.Windows.Input;

namespace HuTaoHelper.Visual.View.Utils;

public class StaticCommands {
	public static Action TrayExitAction = () => { };
	
	public static ICommand TrayExitCommand { get; } = new DelegateCommand {
		CommandAction = () => TrayExitAction()
	};
}