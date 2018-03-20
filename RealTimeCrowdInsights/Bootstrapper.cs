using Caliburn.Micro;
using RealTimeCrowdInsights.ViewModels;
using System.Windows;

namespace RealTimeCrowdInsights
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
