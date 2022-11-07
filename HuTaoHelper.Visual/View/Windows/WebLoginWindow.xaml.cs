using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Localization;
using HuTaoHelper.Visual.Control;
using HuTaoHelper.Visual.View.Utils;
using Constants = HuTaoHelper.Core.Core.Constants;

namespace HuTaoHelper.Visual.View.Windows;

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

	private static async Task NoAuthenticationAsync() {
		await ViewUtils.ShowMessageAsync(Translations.LocWebLoginNoAuthentication, ViewUtils.DIALOG_WEB_LOGIN);
	}

	private async void Save_OnClick(object sender, RoutedEventArgs e) {
		var cookies = await Browser.CoreWebView2.CookieManager.GetCookiesAsync(null);
		if (cookies == null) {
			await NoAuthenticationAsync();
			return;
		}
		
		var remapped = new Dictionary<string, string>();
		foreach (var cookie in cookies) {
			remapped[cookie.Name] = cookie.Value;
		}

		if (Account.Cookies.ParseFrom(remapped)) {
			Settings.Save();
			DialogResult = true;
		} else {
			await NoAuthenticationAsync();
		}
	}
}