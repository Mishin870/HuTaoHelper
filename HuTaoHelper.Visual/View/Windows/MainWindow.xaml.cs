using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Localization;
using HuTaoHelper.Core.Web.Tools;
using HuTaoHelper.Notifications.Registry;
using HuTaoHelper.Notifications.Target;
using HuTaoHelper.Visual.Control;
using HuTaoHelper.Visual.Localization;
using HuTaoHelper.Visual.View.Dialogs;
using HuTaoHelper.Visual.View.Utils;
using HuTaoHelper.Visual.View.ViewModels;
using MaterialDesignThemes.Wpf;

namespace HuTaoHelper.Visual.View.Windows;

public partial class MainWindow {
	private readonly SnackbarMessageQueue EventQueue = new();
	private readonly TaskbarIcon AppIcon;

	public MainWindow() {
		InitializeComponent();
		CommandBindings.Add(new CommandBinding(GlobalCommands.CheckIn, DoCheckIn));
		CommandBindings.Add(new CommandBinding(GlobalCommands.AutoLogin, DoAutoLogin));
		ViewCallbacks.CallbackRefreshAccountsList = () => {
			Application.Current.Dispatcher.Invoke((Action)delegate {
				var items = Settings.Instance.Accounts.Values;
				AccountsList.ItemsSource = items;
				CollectionViewSource.GetDefaultView(items).Refresh();
			});
		};
		Application.Current.Exit += Application_OnExit;

		var showCommand = new DelegateCommand {
			CommandAction = () => { SetVisibleState(Visibility == Visibility.Hidden); }
		};
		AppIcon = (TaskbarIcon)FindResource("TrayIcon");
		AppIcon.LeftClickCommand = showCommand;
		StaticCommands.TrayExitAction = () => { Application.Current.Shutdown(); };
	}

	private void Application_OnExit(object sender, ExitEventArgs e) {
		Scheduler.ChannelShutdown.RunAll();
		AppIcon.Dispose();
	}

	private async void DoCheckIn(object sender, ExecutedRoutedEventArgs e) {
		if (e.OriginalSource is FrameworkElement { DataContext: Account account }) {
			await ViewUtils.DoWithPreloaderAsync(() => {
				if (account.Cookies.IsValid()) {
					DailyCheckIn.DoCheckInAsync(account).Wait();
				} else {
					Application.Current.Dispatcher.Invoke((Action)delegate {
						if (Automation.AuthenticateWeb(account)) {
							Settings.Save();
							Logging.PostEvent(Translations.LocAuthenticationSuccessful);
						} else {
							Logging.PostEvent(Translations.LocAuthenticationError);
						}
					});
				}
			});
		}
	}

	private async void DoAutoLogin(object sender, ExecutedRoutedEventArgs e) {
		if (e.OriginalSource is FrameworkElement { DataContext: Account account }) {
			await Automation.DoAutologinAsync(account);
		}
	}

	private void SelectLanguage(CultureInfo culture, bool notification = true) {
		CultureResources.ChangeCulture(culture);

		if (notification) {
			Logging.PostEvent(Translations.LocLanguageChanged);
		}

		Settings.Instance.Language = culture.Name;
		Settings.Save();
	}

	private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
		foreach (var culture in CultureResources.SupportedCultures) {
			var languageItem = new MenuItem {
				Header = culture.DisplayName
			};
			languageItem.Click += (_, _) => { SelectLanguage(culture); };
			LanguagesMenu.Items.Add(languageItem);
		}

		EventsLog.MessageQueue = EventQueue;
		Logging.EventsProcessor = (text, durationMs) => {
			EventQueue.Enqueue($"{text}", null, null,
				null, false, false,
				TimeSpan.FromMilliseconds(durationMs));
		};

		Settings.Load();
		RefreshDailyCheckInTask();

		var selectedLanguage = Settings.Instance.Language;

		foreach (var culture in CultureResources.SupportedCultures) {
			if (culture.Name == selectedLanguage) {
				SelectLanguage(culture, false);
				return;
			}
		}

		SelectLanguage(CultureResources.SupportedCultures[0], false);
	}

	private void MainWindow_OnClosing(object? sender, CancelEventArgs e) {
		Settings.Save();
	}

	private async void RefreshAccountsMenu_OnClick(object sender, RoutedEventArgs e) {
		await ViewUtils.DoWithPreloaderAsync(() => {
			foreach (var account in Settings.Instance.Accounts.Values) {
				account.RefreshGameInformation().Wait();
			}

			Logging.PostEvent(Translations.LocAllAccountsRefreshed);
		});
	}

	private async void AddAccount_OnClick(object sender, RoutedEventArgs e) {
		var addAccountViewModel = new AddAccountViewModel();

		var view = new AddAccountDialog {
			DataContext = addAccountViewModel
		};
		await DialogHost.Show(view, ViewUtils.DIALOG_ROOT,
			null, (_, args) => {
				if (args.Parameter is false) return;

				args.Cancel();

				var account = addAccountViewModel.ToAccount();
				if (account == null) {
					return;
				}

				args.Session.UpdateContent(new PreloaderDialog());

				if (Automation.AuthenticateWeb(account)) {
					account.RefreshGameInformation().WaitAsync(CancellationToken.None);
				}

				Task.Delay(TimeSpan.FromSeconds(1))
					.ContinueWith((_, _) => args.Session.Close(false), null,
						TaskScheduler.FromCurrentSynchronizationContext());
			});
	}

	private void AccountsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		AccountsList.SelectedItem = null;
	}

	private static async Task DoAccountContextAction(RoutedEventArgs e, Func<Account, Task> action,
		bool needPreloader = true) {
		if (e.Source is not MenuItem {
			    Parent: ContextMenu { PlacementTarget: ListViewItem { DataContext: Account account } }
		    }) return;

		if (needPreloader) {
			await ViewUtils.DoWithPreloaderAsync(() => { action(account).Wait(); });
		} else {
			await action(account);
		}
	}

	private async void AccountRemoveMenu_OnClick(object sender, RoutedEventArgs e) {
		await DoAccountContextAction(e, account => {
			Settings.Instance.RemoveAccount(account);
			Settings.Save();
			return Task.CompletedTask;
		});
	}

	private async void AccountReauthenticateMenu_OnClick(object sender, RoutedEventArgs e) {
		await DoAccountContextAction(e, account => {
			Application.Current.Dispatcher.Invoke((Action)delegate { Automation.AuthenticateWeb(account); });
			return Task.CompletedTask;
		});
	}

	private async void AccountRefreshMenu_OnClick(object sender, RoutedEventArgs e) {
		await DoAccountContextAction(e, async account => {
			await account.RefreshGameInformation();
			Logging.PostEvent(Translations.LocAccountRefreshed
				.Replace("$1", account.Name));
		});
	}

	private async void AccountChangeTitleMenu_OnClick(object sender, RoutedEventArgs e) {
		await DoAccountContextAction(e, async account => {
			var changeTitleViewModel = new ChangeTitleViewModel {
				Account = account,
				Title = account.Title
			};

			var view = new ChangeTitleDialog {
				DataContext = changeTitleViewModel
			};
			await DialogHost.Show(view, ViewUtils.DIALOG_ROOT,
				null, (_, args) => {
					if (args.Parameter is false) return;

					account.Title = changeTitleViewModel.Title;
					if (string.IsNullOrWhiteSpace(account.Title)) {
						account.Title = null;
					}

					Settings.Save();
				});
		}, false);
	}

	private void RefreshDailyCheckInTask() {
		DailyCheckInTask.IsChecked = TasksSchedulerHelper.IsExist(TasksSchedulerHelper.TASK_DAILY_CHECKIN);
	}

	private void DailyCheckInTask_OnClick(object sender, RoutedEventArgs e) {
		var name = TasksSchedulerHelper.TASK_DAILY_CHECKIN;

		if (TasksSchedulerHelper.IsExist(name)) {
			TasksSchedulerHelper.Delete(name);
		} else {
			TasksSchedulerHelper.Create(name, "daily", "Task for automatic daily check-ins");
		}

		RefreshDailyCheckInTask();
	}

	private void MainWindow_OnStateChanged(object? sender, EventArgs e) {
		if (WindowState == WindowState.Minimized) {
			SetVisibleState(false);
		}
	}

	private void SetVisibleState(bool visible) {
		if (visible) {
			Show();
			WindowState = WindowState.Normal;
		} else {
			Hide();
		}
	}

	private async void NotificationTargets_OnClick(object sender, RoutedEventArgs e) {
		var model = new NotificationTargetsViewModel {
			Targets = Settings.Instance.Notifications
				.ToDictionary(x=>x.Key, y=>y.Value)
		};

		var view = new NotificationTargetsDialog {
			DataContext = model
		};

		AddNotificationTargetViewModel? addModel = null;
		NotificationTargetCardViewModel? cardModel = null;

		await DialogHost.Show(view, ViewUtils.DIALOG_ROOT,
			null, (_, args) => {
				if (args.Parameter is false) return;

				args.Cancel();

				var command = DialogExitCommand.NONE;
				object? parameter = null;

				if (args.Parameter is DialogExitCommand dialogExitCommand) {
					command = dialogExitCommand;
				} else if (args.Parameter is DialogExitContainer container) {
					command = container.Command;
					parameter = container.Parameter;
				}

				if (command == DialogExitCommand.ADD_NOTIFICATION_TARGET) {
					var types = NotificationsRegistry.AllTypes();
					addModel = new AddNotificationTargetViewModel {
						Types = types,
						Target = NotificationsRegistry.Build(types[0])
					};

					args.Session.UpdateContent(new AddNotificationTargetDialog {
						DataContext = addModel
					});
				} else if (command == DialogExitCommand.SHOW_NOTIFICATION_TARGET) {
					if (parameter is KeyValuePair<string, INotificationTarget> pair) {
						cardModel = new NotificationTargetCardViewModel {
							Code = pair.Key,
							Target = pair.Value
						};

						args.Session.UpdateContent(new NotificationTargetCardDialog {
							DataContext = cardModel
						});
					}
				} else if (command == DialogExitCommand.DELETE_NOTIFICATION_TARGET) {
					if (cardModel != null) {
						Application.Current.Dispatcher.Invoke((Action)delegate {
							Settings.Instance.RemoveNotificationTarget(cardModel.Code);							
						});

						args.Session.UpdateContent(new PreloaderDialog());
						Task.Delay(TimeSpan.FromSeconds(1))
							.ContinueWith((_, _) => {
									Logging.PostEvent(Translations.LocNotificationRemoved
										.Replace("$1", cardModel.Code));
									cardModel = null;
									args.Session.Close(false);
								}, null,
								TaskScheduler.FromCurrentSynchronizationContext());
					}
				} else if (command == DialogExitCommand.ADD_NOTIFICATION_TARGET_FINAL) {
					if (addModel != null) {
						if (string.IsNullOrWhiteSpace(addModel.Code)) return;
						if (Settings.Instance.GetNotificationTarget(addModel.Code) != null) return;
						if (addModel.Target == null) return;
						if (!addModel.Target.IsValid()) return;

						Application.Current.Dispatcher.Invoke((Action)delegate {
							Settings.Instance.AddNotificationTarget(addModel.Code, addModel.Target);							
						});

						args.Session.UpdateContent(new PreloaderDialog());
						Task.Delay(TimeSpan.FromSeconds(1))
							.ContinueWith((_, _) => {
									Logging.PostEvent(Translations.LocNotificationCreated
										.Replace("$1", addModel.Code));
									addModel = null;
									args.Session.Close(false);
								}, null,
								TaskScheduler.FromCurrentSynchronizationContext());
					}
				}
			});
	}
}