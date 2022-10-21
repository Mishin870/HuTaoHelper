using System.Collections.Generic;
using HuTaoHelper.Control;
using HuTaoHelper.Core;

namespace HuTaoHelper.View.ViewModels; 

public class GameHandleSelectionViewModel : ViewModelBase {
	public Account Account { get; set; } = null!;
	public List<IGameHandle> Handles { get; set; } = new();
}