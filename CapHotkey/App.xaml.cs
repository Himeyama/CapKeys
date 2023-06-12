using Microsoft.UI.Xaml;

namespace CapHotkey
{
    public partial class App : Application
    {
        private MainWindow? mainWindow;
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {

            mainWindow = new MainWindow();
            mainWindow.Activate();
            
            // capKeyBoard.UnHook();
        }

        
    }
}
