namespace HuTaoHelper.Core; 

public class Settings {
	public static Settings Instance { get; set; } = null!;

	public AutologinSettings Autologin;
	
	public static void Save() {
		
	}

	public static void Load() {
		Instance = new Settings {
			Autologin = new AutologinSettings()
		};
	}
}