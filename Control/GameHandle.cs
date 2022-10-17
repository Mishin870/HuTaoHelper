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

	/// <summary>
	/// Title of the game for the selection if there are more than one game
	/// </summary>
	/// <returns></returns>
	string Title();
}

/// <summary>
/// Common base for most game handles
/// </summary>
public abstract class BaseGameHandle : IGameHandle {
	public abstract Task AutologinAsync(Account account);
	public abstract string Title();

	protected static async Task SimpleLoginAsync(Process process, Account account) {
		var settings = Settings.Instance.Autologin;
		Keyboard.SwitchToEnglish();
		
		// Bring game to front
		WindowHelper.BringProcessToFront(process);
		await Task.Delay(settings.DelayBringToTop);
		
		// Selecting all text to ensure we're typing in the clear textfield
		Keyboard.Press(Key.LeftCtrl);
		Keyboard.Type(Key.A);
		Keyboard.Release(Key.LeftCtrl);
		await Task.Delay(settings.DelaySelectAll);
		
		Keyboard.Type(account.Login);
		await Task.Delay(settings.DelayInputLogin);

		Keyboard.Type(Key.Tab);
		await Task.Delay(settings.DelayTabAfterInputLogin);
		
		Keyboard.Type(account.Password);
		await Task.Delay(settings.DelayInputPassword);
		
		Keyboard.Type(Key.Enter);
	}

	public string HandleTitle => Title();
	public string HandleTitleCharacter => Title()[..1];
}

/// <summary>
/// Handle for game running on the user device natively
/// </summary>
public class NativeGameHandle : BaseGameHandle {
	private readonly Process Process;

	public NativeGameHandle(Process process) {
		Process = process;
	}
	
	public override Task AutologinAsync(Account account) {
		return SimpleLoginAsync(Process, account);
	}

	public override string Title() {
		return "Genshin Impact";
	}
}

/// <summary>
/// Handle for game running on the GeForce NOW cloud
/// </summary>
public class GeForceNowGameHandle : BaseGameHandle {
	private readonly Process Process;

	public GeForceNowGameHandle(Process process) {
		Process = process;
	}

	public override Task AutologinAsync(Account account) {
		return SimpleLoginAsync(Process, account);
	}

	public override string Title() {
		return "GeForce NOW - Genshin Impact";
	}
}