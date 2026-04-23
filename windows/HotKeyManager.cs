using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DeSicaBar;

sealed class HotKeyManager : NativeWindow, IDisposable
{
    const int WM_HOTKEY = 0x0312;

    [DllImport("user32.dll")] static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    [DllImport("user32.dll")] static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    const uint MOD_ALT = 0x0001;
    const uint MOD_CONTROL = 0x0002;

    public event Action<int>? HotKeyPressed;

    readonly List<int> registeredIds = new();

    public HotKeyManager()
    {
        CreateHandle(new CreateParams());
    }

    public bool Register(int id, uint vk)
    {
        if (!RegisterHotKey(Handle, id, MOD_CONTROL | MOD_ALT, vk)) return false;
        registeredIds.Add(id);
        return true;
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_HOTKEY)
            HotKeyPressed?.Invoke(m.WParam.ToInt32());
        base.WndProc(ref m);
    }

    public void Dispose()
    {
        foreach (var id in registeredIds)
            UnregisterHotKey(Handle, id);
        DestroyHandle();
    }
}
