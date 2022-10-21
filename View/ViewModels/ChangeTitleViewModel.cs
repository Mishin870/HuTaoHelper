using System.Diagnostics.CodeAnalysis;
using HuTaoHelper.Core;

namespace HuTaoHelper.View.ViewModels;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ChangeTitleViewModel : ViewModelBase {
	public Account Account { get; set; } = null!;
	
	private string? title = "";

	public string? Title {
		get => title;
		set => SetProperty(ref title, value);
	}
}