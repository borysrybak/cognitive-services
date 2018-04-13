using System.Windows;

namespace RealTimeFaceInsights
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            RealTimeFaceInsights.Properties.Settings.Default.Save();
        }
    }
}
