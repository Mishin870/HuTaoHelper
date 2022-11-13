using System.Diagnostics.CodeAnalysis;
using HuTaoHelper.Notifications.Target;

namespace HuTaoHelper.Visual.Notifications; 

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class TelegramNotificationsViewModel : NotificationsViewModel {
	private string botToken = "";
	private long chatId = 0;

	public string BotToken {
		get => botToken;
		set => SetProperty(ref botToken, value);
	}
	
	public long ChatId {
		get => chatId;
		set => SetProperty(ref chatId, value);
	}

	public override INotificationTarget BuildTarget() {
		return new TelegramNotificationTarget {
			BotToken = botToken,
			ChatId = chatId,
		};
	}

	public override string NotificationType() {
		return new TelegramNotificationTarget().NotificationType();
	}
}