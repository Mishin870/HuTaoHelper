using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HuTaoHelper.Control;
using HuTaoHelper.Core;
using HuTaoHelper.Web;

namespace HuTaoHelper.Windows; 

public partial class MainWindow {
	public MainWindow() {
		InitializeComponent();
		CommandBindings.Add(new CommandBinding(GlobalCommands.CheckIn, DoCheckIn));
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
				RefreshAccounts();
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
		RefreshAccounts();
	}

	public void RefreshAccounts() {
		AccountsList.ItemsSource = Settings.Instance.Accounts.Values;
	}

	private async void AccountsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (AccountsList.SelectedItem is Account account) {
			await Automation.DoAutologinAsync(account);
			AccountsList.SelectedItem = null;
		}
	}

	private async void RefreshAccountsMenu_OnClick(object sender, RoutedEventArgs e) {
		foreach (var account in Settings.Instance.Accounts.Values) {
			await account.RefreshGameInformation();
		}
		
		Logging.PostEvent("Accounts information refreshed");
		RefreshAccounts();
	}
}