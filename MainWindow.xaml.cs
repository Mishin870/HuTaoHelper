using System.Windows;
using System.Windows.Controls;
using HuTaoHelper.Control;
using HuTaoHelper.Core;

namespace HuTaoHelper {
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
			Settings.Load();
			RefreshAccounts();
		}

		public void RefreshAccounts() {
			AccountsList.ItemsSource = Settings.Instance.Accounts.Values;
			AccountsList.SelectionChanged += AccountsListOnSelectionChanged;
		}

		private async void AccountsListOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (AccountsList.SelectedItem is Account account) {
				await Automation.DoAutologinAsync(account);
				AccountsList.SelectedItem = null;
			}
		}
	}
}