using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using H.Hooks;
using System.IO;

namespace CapHotkey
{
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);


            KeyHook();
            // while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            // {
        }

        void ClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        int KeyHook(){
            var keyboardHook = new LowLevelKeyboardHook
            {
                IsCapsLock = true,
                IsLeftRightGranularity = true,
                IsExtendedMode = true,
                HandleModifierKeys = true,
            };
            keyboardHook.Up += (_, args) =>
            {
                string log = $"{nameof(keyboardHook)}.{nameof(keyboardHook.Up)}: All keys: {args.Keys}. Current key: {args.CurrentKey}";
                // File.WriteAllText("log", log);
                KeyLog.Text = log;
            };
            keyboardHook.Down += (_, args) =>
            {
                string log = $"{nameof(keyboardHook)}.{nameof(keyboardHook.Up)}: All keys: {args.Keys}. Current key: {args.CurrentKey}";
                // File.WriteAllText("log", log);
                // KeyLog.Text = log;
            };
            keyboardHook.Start();
            return 0;
        }

        void KeyInput(string key)
        {
            KeyLog.Text = key;
        }
    }
}