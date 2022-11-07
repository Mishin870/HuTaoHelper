using System;
using System.Threading.Tasks;
using HuTaoHelper.Visual.View.Dialogs;
using MaterialDesignThemes.Wpf;

namespace HuTaoHelper.Visual.View.Utils;

/// <summary>
/// GUI utils
/// </summary>
public static class ViewUtils {
	public const string DIALOG_ROOT = "RootDialog";
	public const string DIALOG_WEB_LOGIN = "WebLoginDialog";

	/// <summary>
	/// Show simple message popup with cancel button
	/// </summary>
	/// <param name="text">Message text</param>
	/// <param name="host">Dialog host to use</param>
	public static async Task ShowMessageAsync(string text, string host = DIALOG_ROOT) {
		var dialog = new MessageDialog {
			Message = { Text = text }
		};

		await DialogHost.Show(dialog, host);
	}

	/// <summary>
	/// Show preloader and do some long action
	/// </summary>
	/// <param name="action">Action to do with preloader</param>
	/// <param name="host">Dialog host to use</param>
	public static async Task DoWithPreloaderAsync(Action action, string host = DIALOG_ROOT) {
		var dialog = new PreloaderDialog();

		await DialogHost.Show(dialog, host, (DialogOpenedEventHandler) ((sender, args) => {
			Task.Run(action)
				.ContinueWith((_, _) => args.Session.Close(false), null,
					TaskScheduler.FromCurrentSynchronizationContext());
		}));
	}
}