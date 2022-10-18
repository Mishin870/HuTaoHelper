using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace HuTaoHelper.Control;

/// <summary>
/// Keyboard utilities
/// </summary>
public static class Keyboard {
	private static readonly Key[] ExtendedKeys = {
		Key.RightAlt,
		Key.RightCtrl,
		Key.NumLock,
		Key.Insert,
		Key.Delete,
		Key.Home,
		Key.End,
		Key.Prior,
		Key.Next,
		Key.Up,
		Key.Down,
		Key.Left,
		Key.Right,
		Key.Apps,
		Key.RWin,
		Key.LWin
	};

	/// <summary>
	/// Switch system keyboard layout to any installed English layout
	/// </summary>
	public static void SwitchToEnglish() {
		SwitchTo("eng");
	}

	private static void SwitchTo(string isoThreeLetterCode) {
		var manager = InputLanguageManager.Current;
		
		if (manager.AvailableInputLanguages == null) return;
		foreach (var language in manager.AvailableInputLanguages.Cast<CultureInfo>()) {
			if (language.ThreeLetterISOLanguageName == isoThreeLetterCode) {
				manager.CurrentInputLanguage = language;
				return;
			}
		}
	}

	public static void Press(Key key) {
		SendKeyboardInput(key, press: true);
	}

	public static void Release(Key key) {
		SendKeyboardInput(key, press: false);
	}

	public static void Reset() {
		foreach (Key value in Enum.GetValues(typeof(Key))) {
			if (value != 0 && (int)(System.Windows.Input.Keyboard.GetKeyStates(value) & KeyStates.Down) > 0) {
				Release(value);
			}
		}
	}

	public static void Type(Key key) {
		Press(key);
		Release(key);
	}

	public static void Type(string text) {
		foreach (var ch in text) {
			int num = NativeMethods.VkKeyScan(ch);
			var flag = (num & 0x100) == 256;
			var key = KeyInterop.KeyFromVirtualKey(num & 0xFF);
			if (flag) {
				Type(key, new[] { Key.LeftShift });
			} else {
				Type(key);
			}
		}
	}

	private static void Type(Key key, Key[] modifierKeys) {
		foreach (var key2 in modifierKeys) {
			Press(key2);
		}

		Type(key);
		foreach (var item in modifierKeys.Reverse()) {
			Release(item);
		}
	}
	
	private static void SendKeyboardInput(Key key, bool press) {
		var mi = default(NativeMethods.Input);
		mi.Type = 1;
		mi.Union.keyboardInput.WVk = (short)KeyInterop.VirtualKeyFromKey(key);
		mi.Union.keyboardInput.WScan = (short)NativeMethods.MapVirtualKey(mi.Union.keyboardInput.WVk, 0);
		var num = 0;
		if (mi.Union.keyboardInput.WScan > 0) {
			num |= 8;
		}

		if (!press) {
			num |= 2;
		}

		mi.Union.keyboardInput.DwFlags = num;
		if (ExtendedKeys.Contains(key)) {
			mi.Union.keyboardInput.DwFlags |= 1;
		}

		mi.Union.keyboardInput.Time = 0;
		mi.Union.keyboardInput.DwExtraInfo = new IntPtr(0);
		if (NativeMethods.SendInput(1, ref mi, Marshal.SizeOf(mi)) == 0) {
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
	}
}