using System.Collections.Generic;
using HuTaoHelper.Visual.Notifications;

namespace HuTaoHelper.Visual.View.ViewModels;

public class AddNotificationTargetViewModel : ViewModelBase {
	public List<string> Types { get; set; }
	public NotificationsViewModel? Target { get; set; }
}