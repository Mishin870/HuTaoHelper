using System;
using System.Windows;
using HuTaoHelper.Control;
using HuTaoHelper.Core;

namespace HuTaoHelper;

public partial class HoyolabLoginWindow {
	private readonly Account Account;

	public HoyolabLoginWindow(Account account) {
		Account = account;
		InitializeComponent();
	}

	private async void TestWindow_OnLoaded(object sender, RoutedEventArgs e) {
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
		
		Browser.Source = new Uri("https://hoyolab.com/home");
	}

	private void NoAuthentication() {
		MessageBox.Show("I don't see authentication :(");
	}

	private async void Save_OnClick(object sender, RoutedEventArgs e) {
		var cookies = await Browser.CoreWebView2.CookieManager.GetCookiesAsync(null);
		if (cookies == null) {
			NoAuthentication();
			return;
		}

		string ltoken = null!;
		string ltuid = null!;
		
		foreach (var cookie in cookies) {
			switch (cookie.Name) {
				case "ltoken":
					ltoken = cookie.Value;
					break;
				case "ltuid":
					ltuid = cookie.Value;
					break;
			}
		}

		if (ltoken == null || ltuid == null) {
			NoAuthentication();
			return;
		}

		Account.LToken = ltoken;
		Account.LTuid = ltuid;

		DialogResult = true;
	}
}