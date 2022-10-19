using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HuTaoHelper.Control;
using HuTaoHelper.Core;
using HuTaoHelper.View;
using HuTaoHelper.Web;

namespace HuTaoHelper.Windows;

public partial class MainWindow {
	public MainWindow() {
		InitializeComponent();
		CommandBindings.Add(new CommandBinding(GlobalCommands.CheckIn, DoCheckIn));
		CommandBindings.Add(new CommandBinding(GlobalCommands.AutoLogin, DoAutoLogin));
		ViewCallbacks.RefreshAccountsList = RefreshAccounts;
	}

	private async void DoCheckIn(object sender, ExecutedRoutedEventArgs e) {
		if (e.OriginalSource is FrameworkElement { DataContext: Account account }) {
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
	}

	private async void DoAutoLogin(object sender, ExecutedRoutedEventArgs e) {
		if (e.OriginalSource is FrameworkElement { DataContext: Account account }) {
			await Automation.DoAutologinAsync(account);
		}
	}

	private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
		EventsLog.MessageQueue = Logging.EventQueue;
		Settings.Load();
	}

	private void MainWindow_OnClosing(object? sender, CancelEventArgs e) {
		Settings.Save();
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

	private async void AddAccount_OnClick(object sender, RoutedEventArgs e) {
		var window = new AddAccountWindow();
		window.ShowDialog();

		if (window.Account != null) {
			if (Automation.AuthenticateApi(window.Account)) {
				await window.Account.RefreshGameInformation();
			}
		}
	}

	private void AccountsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		AccountsList.SelectedItem = null;
	}

	private static void DoAccountContextAction(RoutedEventArgs e, Action<Account> action) {
		if (e.Source is not MenuItem {
			    Parent: ContextMenu { PlacementTarget: ListViewItem { DataContext: Account account } }
		    }) return;

		action(account);
	}

	private void AccountRemoveMenu_OnClick(object sender, RoutedEventArgs e) {
		DoAccountContextAction(e, account => {
			Settings.Instance.RemoveAccount(account);
			Settings.Save();
		});
	}
}