namespace HuTaoHelper.Visual.View.Utils; 

public enum DialogExitCommand {
	NONE,
	ADD_NOTIFICATION_TARGET,
	ADD_NOTIFICATION_TARGET_FINAL,
	SHOW_NOTIFICATION_TARGET,
	DELETE_NOTIFICATION_TARGET,
}

public class DialogExitContainer {
	public DialogExitCommand Command;
	public object? Parameter;
}