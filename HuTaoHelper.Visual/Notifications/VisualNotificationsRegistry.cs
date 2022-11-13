using System;
using System.Collections.Generic;
using System.Linq;

namespace HuTaoHelper.Visual.Notifications; 

public static class VisualNotificationsRegistry {
	private static readonly Dictionary<string, Func<NotificationsViewModel>> NotificationFactories = new();

	static VisualNotificationsRegistry() {
		Register(() => new TelegramNotificationsViewModel());
	}

	public static void Register(Func<NotificationsViewModel> factory) {
		var instance = factory();
		NotificationFactories[instance.NotificationType()] = factory;
	}

	public static NotificationsViewModel Build(string notificationType) {
		if (!NotificationFactories.ContainsKey(notificationType)) {
			throw new ArgumentException($"Notification type {notificationType} doesn't exist");
		}

		return NotificationFactories[notificationType]();
	}

	public static List<string> AllTypes() {
		return NotificationFactories.Keys.ToList();
	}
}