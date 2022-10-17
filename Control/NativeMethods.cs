using System;
using System.Runtime.InteropServices;

namespace HuTaoHelper.Control;

/// <summary>
/// Wrapper for all windows native methods
/// </summary>
internal static class NativeMethods {
	internal struct Input {
		internal int Type;
		internal Inputunion Union;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct Inputunion {
		[FieldOffset(0)] internal Mouseinput mouseInput;
		[FieldOffset(0)] internal Keybdinput keyboardInput;
	}

	internal struct Mouseinput {
		#pragma warning disable CS0649
		internal int Dx;
		internal int Dy;
		internal int MouseData;
		internal int DwFlags;
		internal int Time;
		internal IntPtr DwExtraInfo;
		#pragma warning restore CS0649
	}

	internal struct Keybdinput {
		internal short WVk;
		internal short WScan;
		internal int DwFlags;
		internal int Time;
		internal IntPtr DwExtraInfo;
	}

	[Flags]
	internal enum SendMouseInputFlags {
		MOVE = 0x1,
		LEFT_DOWN = 0x2,
		LEFT_UP = 0x4,
		RIGHT_DOWN = 0x8,
		RIGHT_UP = 0x10,
		MIDDLE_DOWN = 0x20,
		MIDDLE_UP = 0x40,
		X_DOWN = 0x80,
		X_UP = 0x100,
		WHEEL = 0x800,
		ABSOLUTE = 0x8000
	}
	
	internal const int V_KEY_SHIFT_MASK = 256;
	internal const int V_KEY_CHAR_MASK = 255;
	internal const int KEYEVENTF_EXTENDEDKEY = 1;
	internal const int KEYEVENTF_KEYUP = 2;
	internal const int KEYEVENTF_SCANCODE = 8;
	internal const int MOUSEEVENTF_VIRTUALDESK = 16384;
	internal const int SM_XVIRTUALSCREEN = 76;
	internal const int SM_YVIRTUALSCREEN = 77;
	internal const int SM_CXVIRTUALSCREEN = 78;
	internal const int SM_CYVIRTUALSCREEN = 79;
	internal const int X_BUTTON1 = 1;
	internal const int X_BUTTON2 = 2;
	internal const int WHEEL_DELTA = 120;
	internal const int INPUT_MOUSE = 0;
	internal const int INPUT_KEYBOARD = 1;

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	internal static extern int GetSystemMetrics(int nIndex);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	internal static extern int MapVirtualKey(int nVirtKey, int nMapType);

	[DllImport("user32.dll", SetLastError = true)]
	internal static extern int SendInput(int nInputs, ref Input mi, int cbSize);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	internal static extern short VkKeyScan(char ch);

	[DllImport("User32.dll")]
	internal static extern bool SetForegroundWindow(IntPtr handle);

	[DllImport("User32.dll")]
	internal static extern bool ShowWindow(IntPtr handle, int nCmdShow);

	[DllImport("User32.dll")]
	internal static extern bool IsIconic(IntPtr handle);
}