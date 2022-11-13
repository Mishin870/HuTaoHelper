using System.Collections.Generic;
using HuTaoHelper.Notifications.Target;

namespace HuTaoHelper.Visual.View.ViewModels; 

public class NotificationTargetsViewModel : ViewModelBase {
	public Dictionary<string, INotificationTarget> Targets { get; set; } = new();
}