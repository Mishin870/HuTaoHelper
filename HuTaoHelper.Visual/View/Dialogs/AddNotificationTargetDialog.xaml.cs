using System.Windows;
using System.Windows.Controls;
using HuTaoHelper.Notifications.Registry;
using HuTaoHelper.Visual.View.ViewModels;

namespace HuTaoHelper.Visual.View.Dialogs; 

public partial class AddNotificationTargetDialog {
	public AddNotificationTargetDialog() {
		InitializeComponent();
	}

	private void AddNotificationTargetDialog_OnLoaded(object sender, RoutedEventArgs e) {
		TypeSelector.SelectedIndex = 0;
	}

	private void TypeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (DataContext is AddNotificationTargetViewModel model) {
			model.Target = NotificationsRegistry.Build((string)TypeSelector.SelectedItem);
			Form.DataContext = model.Target;
			Form.Content = model.Target;
		}
	}
}