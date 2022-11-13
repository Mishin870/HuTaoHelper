using System.Windows;

namespace HuTaoHelper.Visual.View.Dialogs; 

public partial class AddNotificationTargetDialog {
	public AddNotificationTargetDialog() {
		InitializeComponent();
	}

	private void AddNotificationTargetDialog_OnLoaded(object sender, RoutedEventArgs e) {
		TypeSelector.SelectedIndex = 0;
	}
}