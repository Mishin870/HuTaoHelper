using System.Collections.Generic;
using HuTaoHelper.Notifications.Target;
using HuTaoHelper.Visual.View.Utils;

namespace HuTaoHelper.Visual.View.ViewModels; 

public class NotificationTargetsViewModel : ViewModelBase {
	public Dictionary<string, INotificationTarget> Targets { get; set; } = new();
	public bool IsSelecting { get; set; }
	
	public DelegateCommand ClearCommand { get; } = new() {
		CommandAction = () => { }
	};
}