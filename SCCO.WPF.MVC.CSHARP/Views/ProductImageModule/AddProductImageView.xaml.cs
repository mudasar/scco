using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.ProductImageModule
{
    public partial class AddProductImageView
    {
        private readonly ProductImage _newItem;
        public AddProductImageView()
        {
            InitializeComponent();
            _newItem = new ProductImage();
            DataContext = _newItem;

            ProductCodeSearchBox.Click += (sender, args) => FindProductCode();
            btnAdd.Click += (sender, e) => AddProduct();
        }

        public ProductImage NewItem
        {
            get { return _newItem; }
        }

        private void AddProduct()
        {
            if (string.IsNullOrEmpty(_newItem.ProductCode))
            {
                MessageWindow.ShowAlertMessage("Product Code is required");
                return;
            }

            if (ProductImage.WhereProductCodeIs(_newItem.ProductCode) != null)
            {
                MessageWindow.ShowAlertMessage("Product Code already exists");
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

        private void FindProductCode()
        {
            var account = Controllers.MainController.SearchAccount();
            if (account == null) return;
            _newItem.Title = account.AccountCode;
        }
    }
}
