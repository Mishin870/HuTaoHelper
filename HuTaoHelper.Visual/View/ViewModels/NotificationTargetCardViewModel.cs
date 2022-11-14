using HuTaoHelper.Notifications.Target;

namespace HuTaoHelper.Visual.View.ViewModels; 

public class NotificationTargetCardViewModel : ViewModelBase {
	public string Code { get; set; }
	public INotificationTarget Target { get; set; }
}