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

            UpdateButton.Click += (sender, args) =>
            {
                var result = AboutProject.Controller.CheckUpdates();
                if (result.Success)
                {
                    AboutProject.Controller.UpdateProject();
                }
                else
                {
                    MessageWindow.ShowNotifyMessage(result.Message);
                }
            };
        }
    }
}
