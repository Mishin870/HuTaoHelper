using System.Windows.Controls;
using HuTaoHelper.Control;
using HuTaoHelper.View.ViewModels;
using MaterialDesignThemes.Wpf;

namespace HuTaoHelper.View.Dialogs; 

public partial class GameHandleSelectionDialog {
	public GameHandleSelectionDialog() {
		InitializeComponent();
	}

	private async void HandlesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (HandlesList.SelectedItem is IGameHandle handle) {
			var viewModel = (GameHandleSelectionViewModel)DataContext;
			await handle.AutologinAsync(viewModel.Account);
			DialogHost.CloseDialogCommand.Execute(null,null);
		}
	}
}