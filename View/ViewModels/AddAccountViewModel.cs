using System.Diagnostics.CodeAnalysis;

namespace HuTaoHelper.View.ViewModels;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class AddAccountViewModel : ViewModelBase {
	private string? login = "";
	private string? password = "";
	private string? title = "";

	public string? Login {
		get => login;
		set => SetProperty(ref login, value);
	}
	
	public string? Password {
		get => password;
		set => SetProperty(ref password, value);
	}
	
	public string? Title {
		get => title;
		set => SetProperty(ref title, value);
	}
}