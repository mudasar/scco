using System;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class CompanyView
    {
        private Company _currentItem;

        public CompanyView()
        {
            InitializeComponent();
            _currentItem = new Company();
            _currentItem.Find(1);
            DataContext = _currentItem;

            btnUpdate.Click += (sender, args) => Update();
            btnFindImage.Click += (sender, args) => FindImage();
        }

        private void Update()
        {
            Result result;
            if (_currentItem.ID == 0)
            {
                result = _currentItem.Create();
            }
            else
            {
                result = _currentItem.Update();
            }
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }

            MessageWindow.ShowNotifyMessage("Record updated.");
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

            _currentItem.Image = ImageTool.GetBytesFromImageFile(openFileDialog.FileName);
            ProductLogo.Source = ImageTool.CreateImageSourceFromBytes(_currentItem.Image);
        }
    }
}