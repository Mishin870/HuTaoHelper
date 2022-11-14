using System.Collections.Generic;
using HuTaoHelper.Notifications.Target;

namespace HuTaoHelper.Visual.View.ViewModels;

public class AddNotificationTargetViewModel : ViewModelBase {
	public List<string> Types { get; set; }
	public INotificationTarget? Target { get; set; }
	public string Code { get; set; }
}