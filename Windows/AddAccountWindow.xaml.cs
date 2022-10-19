using System.Windows;
using HuTaoHelper.Core;
using HuTaoHelper.Visual.ViewModels;

namespace HuTaoHelper.Windows; 

public partial class AddAccountWindow {
	public AddAccountViewModel Model { get; set; } = new();
	public Account? Account;
	
	public AddAccountWindow() {
		InitializeComponent();
	}

	private void AddAccountWindow_OnLoaded(object sender, RoutedEventArgs e) {
		DataContext = Model;
	}

	private void AddButton_OnClick(object sender, RoutedEventArgs e) {
		if (string.IsNullOrWhiteSpace(Model.Login) || string.IsNullOrWhiteSpace(Model.Password)) {
			return;
		}
		
		Account = Settings.Instance.CreateAccount(Model.Login, Model.Password);

		if (!string.IsNullOrWhiteSpace(Model.Title)) {
			Account.Title = Model.Title;
		}
		
		Settings.Save();
		DialogResult = true;
	}
}
