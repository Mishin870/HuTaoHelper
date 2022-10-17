using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using HuTaoHelper.Core;

namespace HuTaoHelper.Control; 

/// <summary>
/// Universal game handle for both native and cloud game version
/// </summary>
public interface IGameHandle {
	/// <summary>
	/// Run autologin process with given account
	/// </summary>
	/// <param name="account">Account for login</param>
	/// <returns></returns>
	Task AutologinAsync(Account account);
}

/// <summary>
/// Handle for game running on the user device natively
/// </summary>
public class NativeGameHandle : IGameHandle {
	private readonly Process Process;

	public NativeGameHandle(Process process) {
		Process = process;
	}

	public async Task AutologinAsync(Account account) {
		Keyboard.SwitchToEnglish();
		
		// Bring game to front
		WindowHelper.BringProcessToFront(Process);
		await Task.Delay(500);
		
		// Selecting all text to ensure we're typing in the clear textfield
		Keyboard.Press(Key.LeftCtrl);
		Keyboard.Type(Key.A);
		Keyboard.Release(Key.LeftCtrl);
		await Task.Delay(200);
		
		Keyboard.Type(account.Login);
		await Task.Delay(200);

		Keyboard.Type(Key.Tab);
		await Task.Delay(200);
		
		Keyboard.Type(account.Password);
		await Task.Delay(200);
		
		Keyboard.Type(Key.Enter);
	}
}