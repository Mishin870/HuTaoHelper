using System;
using System.Windows.Input;

namespace HuTaoHelper.Visual.View.Utils;

public class DelegateCommand : ICommand {
	public Action CommandAction { get; init; } = null!;
	public Func<bool>? CanExecuteFunc { get; init; }

	public void Execute(object? parameter) {
		CommandAction();
	}

	public bool CanExecute(object? parameter) {
		return CanExecuteFunc == null || CanExecuteFunc();
	}

	public event EventHandler? CanExecuteChanged {
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}
}