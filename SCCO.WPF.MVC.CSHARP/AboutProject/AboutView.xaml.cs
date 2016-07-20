using SCCO.WPF.MVC.CS.Views;

namespace SCCO.WPF.MVC.CS.AboutProject
{
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
            AboutViewModel viewModel = new AboutViewModel();
            DataContext = viewModel;

            HelpButton.Click += (s, e) =>
            {
                const string url = "https://github.com/Jeralane/scco/wiki";
                System.Diagnostics.Process.Start(url);
            };
        }
    }
}
