namespace HuTaoHelper.Notifications.Target; 

public interface INotificationTarget {
	void Send(object? text);
	string NotificationType();
}