using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CapHotkey
{
    public class KeyEventArgs
    {
        public int keyCode { get; }
        public KeyEventArgs(int keyCode)
        {
            this.keyCode = keyCode;
        }
    }

    public enum KBDLLHOOKSTRUCTFlags
    {
        KEYEVENTF_EXTENDENDKEY = 0x1,
        KEYEVENTF_KEYUP = 0x2,
        KEYEVENTF_SCANCODE = 0x4,
        KEYEVENTF_UNICODE = 0x8,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public KBDLLHOOKSTRUCTFlags flags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    public class CapKeyBoard
    {
        MainWindow? mainWindow;
        public IntPtr hook = IntPtr.Zero;
        const int WH_KEYBOARD_LL = 0x000D;
        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int WM_SYSKEYDOWN = 0x0104;
        const int WM_SYSKEYUP = 0x0105;

        public delegate void KeyEventHandler(object sender, KeyEventArgs e);
        public event KeyEventHandler? KeyDownEvent;
        public event KeyEventHandler? KeyUpEvent;

        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int modKey, int vKey);

        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        private delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        static public CapKeyBoard Init(MainWindow mainWindow)
        {
            CapKeyBoard capKeyBoard = new();
            capKeyBoard.KeyDownEvent += capKeyBoard.KeyDown;
            capKeyBoard.KeyUpEvent += capKeyBoard.KeyUp;
            capKeyBoard.Hook();
            capKeyBoard.mainWindow = mainWindow;
            return capKeyBoard;
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            mainWindow!.Text.Text = $"{e.keyCode}";
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            mainWindow!.Text.Text = $"{e.keyCode}";
        }

        public void Close()
        {
            UnHook();
        }

        public void Hook()
        {
            int WH_KEYBOARD_LL = 0x000D;
            hook = IntPtr.Zero;

            using (Process process = Process.GetCurrentProcess())
            {
                using (ProcessModule? processModule = process!.MainModule)
                {
                    hook = SetWindowsHookEx(
                        WH_KEYBOARD_LL,
                        HookProcedure,
                        GetModuleHandle(processModule!.ModuleName),
                        0
                    );
                }
            }
        }

        IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                KBDLLHOOKSTRUCT? kb = Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT)) as KBDLLHOOKSTRUCT;
                int vkCode = (int)kb!.vkCode;
                OnKeyDownEvent(vkCode);
            }
            else if (nCode >= 0 && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
            {
                KBDLLHOOKSTRUCT? kb = Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT)) as KBDLLHOOKSTRUCT;
                int vkCode = (int)kb!.vkCode;
                OnKeyUpEvent(vkCode);
            }
            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(hook);
            hook = IntPtr.Zero;
        }

        protected void OnKeyDownEvent(int keyCode)
        {
            KeyDownEvent?.Invoke(this, new KeyEventArgs(keyCode));
        }
        protected void OnKeyUpEvent(int keyCode)
        {
            KeyUpEvent?.Invoke(this, new KeyEventArgs(keyCode));
        }


    }
}