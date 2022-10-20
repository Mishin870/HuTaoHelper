using System;
using System.Windows;
using HuTaoHelper.Control;
using HuTaoHelper.Core;

namespace HuTaoHelper.Windows;

public partial class WebLoginWindow {
	private readonly Account Account;

	public WebLoginWindow(Account account) {
		Account = account;
		InitializeComponent();
	}

	private async void WebLoginWindow_OnLoaded(object sender, RoutedEventArgs e) {
		await Automation.ConfigureAccountSessionAsync(Browser, Account);
		
		await Browser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
var checker = setInterval(function () {
	var form = document.getElementsByClassName('login-form-container')[0];
	if (form == null) return;

	var inputs = form.getElementsByTagName('input');

	inputs[0].focus();
	document.execCommand('insertText', false, '" + Account.Login + @"');

	inputs[1].focus();
	document.execCommand('insertText', false, '" + Account.Password + @"');

	clearInterval(checker);
}, 200);
		");
		
		Browser.Source = new Uri(Constants.AuthenticationBrowserSource);
	}

	private void NoAuthentication() {
		MessageBox.Show("I don't see authentication data :(");
	}

	private async void Save_OnClick(object sender, RoutedEventArgs e) {
		var cookies = await Browser.CoreWebView2.CookieManager.GetCookiesAsync(null);
		if (cookies == null) {
			NoAuthentication();
			return;
		}

		if (Account.Cookies.ParseFrom(cookies)) {
			Settings.Save();
			DialogResult = true;
		} else {
			NoAuthentication();
		}
	}
}