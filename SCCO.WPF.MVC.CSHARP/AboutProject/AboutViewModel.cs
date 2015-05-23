using System.ComponentModel;
using System.Reflection;

namespace SCCO.WPF.MVC.CS.AboutProject
{
    class AboutViewModel:INotifyPropertyChanged
    {
        private string _productName;
        private string _version;
        private string _copyright;
        private string _companyName;
        private string _description;
        private string _title;

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value;
                OnPropertyChanged("ProductName");
            }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value;
                OnPropertyChanged("Version");
            }
        }

        public string Copyright
        {
            get { return _copyright; }
            set { _copyright = value;
                OnPropertyChanged("Copyright");
            }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value;
                OnPropertyChanged("CompanyName");
            }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description");}
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title");}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this,new PropertyChangedEventArgs(propertyName));
            }
        }

        public AboutViewModel()
        {
            #region Assembly Attribute Accessors

            // AssemblyTitle
            object[] attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute) attributes[0];
                if (titleAttribute.Title != "")
                {
                    Title = titleAttribute.Title;
                }
            }
            Title = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);

            // AssemblyVersion
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // AssemblyDescription
            attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
            if (attributes.Length == 0)
            {
                Description = "";
            }
            Description = ((AssemblyDescriptionAttribute) attributes[0]).Description;

            // AssemblyProduct
            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                ProductName = "";
            }
            ProductName = ((AssemblyProductAttribute) attributes[0]).Product;

            // AssemblyCopyright
            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
                Copyright = "";
            }
            Copyright = ((AssemblyCopyrightAttribute) attributes[0]).Copyright;

            // AssemblyCompany
            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
                CompanyName = "";
            }
            CompanyName = ((AssemblyCompanyAttribute) attributes[0]).Company;

            #endregion
        }
    }
}
