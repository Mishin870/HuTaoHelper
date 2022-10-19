using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HuTaoHelper.Control;
using HuTaoHelper.Core;
using HuTaoHelper.Visual;
using HuTaoHelper.Web;

namespace HuTaoHelper.Windows;

public partial class MainWindow {
	public MainWindow() {
		InitializeComponent();
		CommandBindings.Add(new CommandBinding(GlobalCommands.CheckIn, DoCheckIn));
		VisualCallbacks.RefreshAccountsList = RefreshAccounts;
	}

	private async void DoCheckIn(object sender, ExecutedRoutedEventArgs e) {
		if (e.OriginalSource is not Button source) {
			return;
		}

		if (source.DataContext is not Account account) {
			return;
		}

		if (account.Cookies.IsValid()) {
			await DailyCheckIn.DoCheckInAsync(account);
		} else {
			if (Automation.AuthenticateApi(account)) {
				Settings.Save();
				Logging.PostEvent("Account succesfully authenticated!");
			} else {
				Logging.PostEvent("Error authenticating account");
			}
		}
	}

	private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
		EventsLog.MessageQueue = Logging.EventQueue;
		Settings.Load();
		Constants.Load();
	}

	public void RefreshAccounts() {
		AccountsList.ItemsSource = null;
		AccountsList.ItemsSource = Settings.Instance.Accounts.Values;
	}

	private async void RefreshAccountsMenu_OnClick(object sender, RoutedEventArgs e) {
		foreach (var account in Settings.Instance.Accounts.Values) {
			await account.RefreshGameInformation();
		}

		Logging.PostEvent("Accounts information refreshed");
	}

	private void MainWindow_OnClosing(object? sender, CancelEventArgs e) {
		Settings.Save();
	}

	private async void AddAccount_OnClick(object sender, RoutedEventArgs e) {
		var window = new AddAccountWindow();
		window.ShowDialog();

		if (window.Account != null) {
			if (Automation.AuthenticateApi(window.Account)) {
				await window.Account.RefreshGameInformation();
			}
		}
	}

	private async void OnAccountClicked(object sender, MouseButtonEventArgs e) {
		if (sender is ListViewItem { DataContext: Account account }) {
			await Automation.DoAutologinAsync(account);
		}
	}

	private void AccountsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		AccountsList.SelectedItem = null;
	}
}