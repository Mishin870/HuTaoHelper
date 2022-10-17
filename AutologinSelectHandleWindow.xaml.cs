using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HuTaoHelper.Control;
using HuTaoHelper.Core;

namespace HuTaoHelper; 

public partial class AutologinSelectHandleWindow {
	private readonly Account Account;
	private readonly List<IGameHandle> Handles;

	public AutologinSelectHandleWindow(Account account, List<IGameHandle> handles) {
		Account = account;
		Handles = handles;
		InitializeComponent();
	}

	private void AutologinSelectHandleWindow_OnLoaded(object sender, RoutedEventArgs e) {
		HandlesList.ItemsSource = Handles;
	}

	private async void HandlesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (HandlesList.SelectedItem is IGameHandle handle) {
			await handle.AutologinAsync(Account);
			DialogResult = true;
		}
	}
}