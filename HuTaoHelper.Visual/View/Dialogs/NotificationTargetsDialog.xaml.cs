using System.Collections.Generic;
using System.Windows.Controls;
using HuTaoHelper.Notifications.Target;

namespace HuTaoHelper.Visual.View.Dialogs; 

public partial class NotificationTargetsDialog {
	public NotificationTargetsDialog() {
		InitializeComponent();
	}

	private async void TargetsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (TargetsList.SelectedItem is KeyValuePair<string, INotificationTarget> pair) {
			TargetsList.UnselectAll();
		}
	}
}