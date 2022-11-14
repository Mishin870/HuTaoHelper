using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using HuTaoHelper.Notifications.Target;

namespace HuTaoHelper.Visual.Notifications;

public class NotificationTargetIcons : MarkupExtension {
	private static readonly Dictionary<string, string> Icons = new() {
		{ new TelegramNotificationTarget().NotificationType(), "/Images/Telegram.png" }
	};

	private readonly object Target;

	public NotificationTargetIcons(object target) {
		Target = target;
	}

	public override object ProvideValue(IServiceProvider serviceProvider) {
		var targetBinding = Target as Binding ?? new Binding { Source = Target };

		var binding = new MultiBinding {
			Converter = new TargetValueConverter()
		};

		binding.Bindings.Add(targetBinding);

		return binding.ProvideValue(serviceProvider);
	}

	private class TargetValueConverter : IMultiValueConverter {
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			if (values[0] is not INotificationTarget target) {
				return "";
			} else {
				var image = Icons.GetValueOrDefault(target.NotificationType(), "/Images/Unknown.png");
				
				return new BitmapImage(
					new Uri($"pack://application:,,,/HuTaoHelper;component{image}", UriKind.Absolute));
			}
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}