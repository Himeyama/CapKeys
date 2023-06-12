using Microsoft.UI.Xaml;

namespace CapHotkey
{
    public sealed partial class MainWindow : Window
    {
        CapKeyBoard capKeyBoard;

        public MainWindow()
        {
            capKeyBoard = CapKeyBoard.Init(this);

            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            Closed += WindowClosed;
        }

        private void WindowClosed(object sender, WindowEventArgs args)
        {
            capKeyBoard.Close();
        }


        void ClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
