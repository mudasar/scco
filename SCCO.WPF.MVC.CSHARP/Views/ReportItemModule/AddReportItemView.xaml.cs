using System;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.ReportItemModule
{
    public partial class AddReportItemView
    {
        private readonly ReportItem _newItem;

        public AddReportItemView()
        {
            InitializeComponent();
            _newItem = new ReportItem();
            DataContext = _newItem;

            btnFindImage.Click += (sender, args) => FindImage();
            btnAdd.Click += (sender, e) => AddProduct();
        }

        public ReportItem NewItem
        {
            get { return _newItem; }
        }

        private void AddProduct()
        {
            if (string.IsNullOrEmpty(_newItem.Title))
            {
                MessageWindow.ShowAlertMessage("Report Title is required.");
                return;
            }

            if (ReportItem.WhereTitleIs(_newItem.Title) != null)
            {
                MessageWindow.ShowAlertMessage("Report Title already exists.");
                return;
            }

            if (string.IsNullOrEmpty(_newItem.Description))
            {
                MessageWindow.ShowAlertMessage("Description is required.");
                return;
            }

            if (string.IsNullOrEmpty(_newItem.Category))
            {
                MessageWindow.ShowAlertMessage("Category is required.");
                return;
            }

            if (string.IsNullOrEmpty(_newItem.ReportFile))
            {
                MessageWindow.ShowAlertMessage("Report File is required.");
                return;
            }

            if (string.IsNullOrEmpty(_newItem.StoredProcedure))
            {
                MessageWindow.ShowAlertMessage("Stored Procedure File is required.");
                return;
            }

            var result = _newItem.Create();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }

            // success
            DialogResult = true;
            Close();
        }

        private void FindImage()
        {
            var openFileDialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Filter = string.Format("Image Files|*.bmp;*.gif;*.jpg; *.png"),
                    DefaultExt = "PNG",
                    Title = String.Format("Select Product Image")
                };
            openFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            _newItem.Image = ImageTool.GetBytesFromImageFile(openFileDialog.FileName);
            ReportLogo.Source = ImageTool.CreateImageSourceFromBytes(_newItem.Image);
        }
    }
}
