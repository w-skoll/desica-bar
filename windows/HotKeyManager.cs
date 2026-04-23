using System.Runtime.InteropServices;

namespace DeSicaBar;

sealed class HotKeyManager : IDisposable
{
    [StructLayout(LayoutKind.Sequential)]
    struct KBDLLHOOKSTRUCT { public uint vkCode, scanCode, flags, time; public IntPtr dwExtraInfo; }

    [DllImport("user32.dll")] static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc fn, IntPtr hMod, uint threadId);
    [DllImport("user32.dll")] static extern bool UnhookWindowsHookEx(IntPtr hook);
    [DllImport("user32.dll")] static extern IntPtr CallNextHookEx(IntPtr hook, int code, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")] static extern short GetKeyState(int vk);

    delegate IntPtr LowLevelKeyboardProc(int code, IntPtr wParam, IntPtr lParam);

    const int WH_KEYBOARD_LL = 13;
    const int WM_KEYDOWN    = 0x0100;
    const int WM_SYSKEYDOWN = 0x0104;
    const int VK_CONTROL    = 0x11;
    const int VK_MENU       = 0x12; // Alt

    readonly Dictionary<uint, int> keyToId = new();
    readonly LowLevelKeyboardProc hookProc; // manteniamo il riferimento per evitare GC
    IntPtr hookHandle;

    public event Action<int>? HotKeyPressed;

    public HotKeyManager()
    {
        hookProc  = HookCallback;
        hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, IntPtr.Zero, 0);
    }

    public void Register(int id, uint vk) => keyToId[vk] = id;

    IntPtr HookCallback(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code >= 0 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
        {
            var kb   = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
            bool ctrl = (GetKeyState(VK_CONTROL) & 0x8000) != 0;
            bool alt  = (GetKeyState(VK_MENU)    & 0x8000) != 0;

            if (ctrl && alt && keyToId.TryGetValue(kb.vkCode, out var id))
            {
                HotKeyPressed?.Invoke(id);
                return (IntPtr)1; // consuma il tasto, non lo passa ad altre app
            }
        }
        return CallNextHookEx(hookHandle, code, wParam, lParam);
    }

    public void Dispose()
    {
        if (hookHandle != IntPtr.Zero)
        {
            UnhookWindowsHookEx(hookHandle);
            hookHandle = IntPtr.Zero;
        }
    }
}
