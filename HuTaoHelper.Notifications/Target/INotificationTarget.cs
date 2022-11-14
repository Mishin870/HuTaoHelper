namespace HuTaoHelper.Notifications.Target; 

public interface INotificationTarget {
	void Send(object? text);
	bool IsValid();
	string NotificationType();
}