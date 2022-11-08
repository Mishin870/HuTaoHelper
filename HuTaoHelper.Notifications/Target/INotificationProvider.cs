namespace HuTaoHelper.Notifications.Target; 

public interface INotificationProvider<in T> where T : INotificationTarget {
	void Send(T target, object? text);
}