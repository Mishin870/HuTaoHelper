using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

		result.AddRange(Process.GetProcessesByName("GenshinImpact")
			.Select(process => new NativeGameHandle(process)));

		return result;
	}
}