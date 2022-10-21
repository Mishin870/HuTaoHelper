using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HuTaoHelper.Core;
using HuTaoHelper.View;
using HuTaoHelper.View.Dialogs;
using HuTaoHelper.View.Utils;
using HuTaoHelper.View.ViewModels;
using HuTaoHelper.View.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Constants = HuTaoHelper.Core.Constants;

namespace HuTaoHelper.Control;

/// <summary>
/// Automation utilities
/// </summary>
public static class Automation {
	/// <summary>
	/// Find any type of running game: native or cloud (e.g. GeForce Now)
	/// </summary>
	/// <returns>List of game handles</returns>
	public static List<IGameHandle> FindGameHandles() {
		var result = new List<IGameHandle>();

		result.AddRange(Process.GetProcessesByName(Constants.GameProcessName)
			.Select(process => new NativeGameHandle(process)));
		
		result.AddRange(Process.GetProcessesByName("GeForceNOW")
			.Where(process => process.MainWindowTitle.StartsWith(Constants.GameName))
			.Select(process => new GeForceNowGameHandle(process)));

		return result;
	}

	public static async Task DoAutologinAsync(Account account) {
		var handles = FindGameHandles();

		switch (handles.Count) {
			case 0:
				Logging.PostEvent("Running game instance not found", 500);
				return;
			case 1:
				await handles[0].AutologinAsync(account);
				return;
			default: {
				var view = new GameHandleSelectionDialog {
					DataContext = new GameHandleSelectionViewModel {
						Account = account,
						Handles = handles
					}
				};
				await DialogHost.Show(view, ViewUtils.DIALOG_ROOT);
				break;
			}
		}
	}

	/// <summary>
	/// Configure WebView to use account profile folder /profiles/account.id
	/// </summary>
	/// <param name="webView2"></param>
	/// <param name="account"></param>
	public static async Task ConfigureAccountSessionAsync(WebView2 webView2, Account account) {
		var profilePath = Path.Join(Directory.GetCurrentDirectory(), "profiles", account.Id.ToString());
		Directory.CreateDirectory(profilePath);

		var environment = await CoreWebView2Environment
			.CreateAsync(null, profilePath);
		await webView2.EnsureCoreWebView2Async(environment);
	}

	/// <summary>
	/// Try to authenticate given account through website. User needs to solve a captcha.
	/// If authentication was successful, then cookies will be saved into <c>Account</c> variable
	/// </summary>
	/// <param name="account"></param>
	/// <returns>Is authentication was successful</returns>
	public static bool AuthenticateWeb(Account account) {
		var window = new WebLoginWindow(account);
		return window.ShowDialog() == true;
	}
}