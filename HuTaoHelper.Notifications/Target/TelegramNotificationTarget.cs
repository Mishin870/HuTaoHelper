using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HuTaoHelper.Notifications.Target; 

public sealed class TelegramNotificationTarget : INotificationTarget {
	public string BotToken { get; set; } = null!;
	public long ChatId { get; set; }

	public void Send(object? text) {
		if (BotToken == null) throw new ArgumentException("BotToken can't be null");
		
		var client = new TelegramBotClient(BotToken);
		client.SendTextMessageAsync(new ChatId(ChatId), $"{text}", ParseMode.Html);
	}

	public bool IsValid() {
		return !string.IsNullOrWhiteSpace(BotToken);
	}

	public string NotificationType() {
		return "telegram";
	}
}