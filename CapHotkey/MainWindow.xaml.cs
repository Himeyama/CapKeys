using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using H.Hooks;
using System.IO;
using Windows.Graphics;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Microsoft.UI;

namespace CapHotkey
{
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            setWindowSize(400, 400);

            KeyHook();
            // while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            // {
        }

        void setWindowSize(int width, int height)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(myWndId);
            appWindow.Resize(new SizeInt32(width, height));
        }

        void ClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Key(string key)
        {
            if (DispatcherQueue.HasThreadAccess)
            {
                KeyLog.Text = key;
            }
            else
            {
                bool isQueued = this.DispatcherQueue.TryEnqueue(
                    Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                    () => KeyLog.Text = key);
            }
        }

        void KeyHook()
        {
            LowLevelKeyboardHook keyboardHook = new();
            keyboardHook.Up += (_, args) =>
            {
                string log = $"{args.CurrentKey}";
                try
                {
                    Key(log);
                }
                catch (Exception ex)
                {
                    File.AppendAllText("log", ex.ToString() + "\n");
                }
            };
            keyboardHook.Down += (_, args) =>
            {
                string log = $"{args.CurrentKey}";

                try
                {
                    // File.AppendAllText("log", log);
                    Key(log);
                }
                catch (Exception ex)
                {
                    File.AppendAllText("log", ex.ToString() + "\n");
                }
            };
            keyboardHook.Start();
        }
    }
}