using System.Collections.Generic;
using System.Windows.Controls;
using HuTaoHelper.Notifications.Target;
using HuTaoHelper.Visual.View.Utils;
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
}