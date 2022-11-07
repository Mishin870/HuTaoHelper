using System.Collections.Generic;
using HuTaoHelper.Core.Core;
using HuTaoHelper.Visual.Control;

namespace HuTaoHelper.Visual.View.ViewModels; 

public class GameHandleSelectionViewModel : ViewModelBase {
	public Account Account { get; set; } = null!;
	public List<IGameHandle> Handles { get; set; } = new();
}