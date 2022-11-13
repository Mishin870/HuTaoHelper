using HuTaoHelper.Notifications.Target;
using HuTaoHelper.Visual.View.ViewModels;

namespace HuTaoHelper.Visual.Notifications; 

public abstract class NotificationsViewModel : ViewModelBase {
	public abstract INotificationTarget BuildTarget();
	public abstract string NotificationType();
}