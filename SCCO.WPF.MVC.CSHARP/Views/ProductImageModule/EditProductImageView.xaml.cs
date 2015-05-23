using System;
using System.Windows;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.ProductImageModule
{
    public partial class EditProductImageView
    {
        private readonly ProductImage _currentItem;

        public EditProductImageView()
        {
            InitializeComponent();

            btnFindImage.Click += (sender, args) => FindImage();
            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => btnFindImage.Focus();
        }

        public EditProductImageView(int id)
            : this()
        {
            _currentItem = new ProductImage();
            _currentItem.Find(id);
            DataContext = _currentItem;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _currentItem.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
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

            _currentItem.Image = ImageTool.GetBytesFromImageFile(openFileDialog.FileName);
            ProductLogo.Source = ImageTool.CreateImageSourceFromBytes(_currentItem.Image);
        }
    }
}

