using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HuTaoHelper.Notifications.Target;
using HuTaoHelper.Visual.View.Utils;
using HuTaoHelper.Visual.View.ViewModels;
using MaterialDesignThemes.Wpf;

namespace HuTaoHelper.Visual.View.Dialogs; 

public partial class NotificationTargetsDialog {
	public NotificationTargetsDialog() {
		InitializeComponent();
	}

	private async void TargetsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (TargetsList.SelectedItem is KeyValuePair<string, INotificationTarget> pair) {
			TargetsList.UnselectAll();
			DialogHost.CloseDialogCommand.Execute(new DialogExitContainer {
				Command = DialogExitCommand.SHOW_NOTIFICATION_TARGET,
				Parameter = pair
			},null);
		}
	}

	private void NotificationTargetsDialog_OnLoaded(object sender, RoutedEventArgs e) {
		var context = (NotificationTargetsViewModel)DataContext;
		
		context.ClearCommand.CommandAction = () => {
			DialogHost.CloseDialogCommand.Execute(new DialogExitContainer {
				Command = DialogExitCommand.SHOW_NOTIFICATION_TARGET,
				Parameter = new KeyValuePair<string,INotificationTarget>("", new TelegramNotificationTarget())
			},null);
		};

		ClearButton.Visibility = context.IsSelecting ? Visibility.Visible : Visibility.Collapsed;
		AddButton.Visibility = context.IsSelecting ? Visibility.Collapsed : Visibility.Visible;
	}
}