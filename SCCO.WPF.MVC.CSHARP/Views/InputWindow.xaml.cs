namespace SCCO.WPF.MVC.CS.Views
{
    public partial class InputWindow
    {
        public InputWindow(string message, string title)
        {
            InitializeComponent();

            FormTitle.Content = title;
            lblMessage.Content = message;

            btnOk.Click += delegate
                               {
                                   DialogResult = true;
                                   InputText = txtInput.Text;
                                   Close();
                               };

            Loaded += (sender, args) => txtInput.Focus();

        }

        public string InputText { get; set; }

    }
}
