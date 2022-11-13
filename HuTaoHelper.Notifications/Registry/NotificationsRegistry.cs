using HuTaoHelper.Notifications.Target;

namespace HuTaoHelper.Notifications.Registry;

public static class NotificationsRegistry {
	private static readonly Dictionary<string, Func<INotificationTarget>> NotificationFactories = new();

	static NotificationsRegistry() {
		Register(() => new TelegramNotificationTarget());
	}

	public static void Register(Func<INotificationTarget> factory) {
		var instance = factory();
		NotificationFactories[instance.NotificationType()] = factory;
	}

	public static INotificationTarget Build(string notificationType) {
		if (!NotificationFactories.ContainsKey(notificationType)) {
			throw new ArgumentException($"Notification type {notificationType} doesn't exist");
		}

		return NotificationFactories[notificationType]();
	}

	public static List<string> AllTypes() {
		return NotificationFactories.Keys.ToList();
	}
}