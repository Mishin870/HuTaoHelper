namespace HuTaoHelper.Core.Core; 

/// <summary>
/// Settings for autologin process (e.g. delays between operations in ms)
/// </summary>
public class AutologinSettings {
	public int DelayBringToTop = 500;
	public int DelaySelectAll = 200;
	public int DelayInputLogin = 200;
	public int DelayTabAfterInputLogin = 200;
	public int DelayInputPassword = 200;
}