using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Localization;
using HuTaoHelper.Notifications.Registry;
using HuTaoHelper.Notifications.Target;
using HuTaoHelper.Visual.View.Dialogs;
using HuTaoHelper.Visual.View.Utils;
using HuTaoHelper.Visual.View.ViewModels;
using MaterialDesignThemes.Wpf;

namespace HuTaoHelper.Visual.Notifications; 

public static class NotificationsVisualController {
	public static async Task ShowTargetsSelector(Action<string, INotificationTarget>? selectionHandler = null) {
		var model = new NotificationTargetsViewModel {
			Targets = Settings.Instance.Notifications
				.ToDictionary(x=>x.Key, y=>y.Value),
			IsSelecting = selectionHandler != null
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
						if (selectionHandler != null) {
							selectionHandler(pair.Key, pair.Value);
							args.Session.UpdateContent(new PreloaderDialog());
							Task.Delay(TimeSpan.FromMilliseconds(500))
								.ContinueWith((_, _) => {
										args.Session.Close(false);
									}, null,
									TaskScheduler.FromCurrentSynchronizationContext());
							return;
						}
						
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
						Task.Delay(TimeSpan.FromMilliseconds(500))
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
						Task.Delay(TimeSpan.FromMilliseconds(500))
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