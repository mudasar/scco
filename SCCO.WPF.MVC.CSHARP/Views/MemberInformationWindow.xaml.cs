using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for MemberInformationWindow.xaml
    /// </summary>
    public partial class MemberInformationWindow
    {
        private Nfmb _member = new Nfmb();

        public MemberInformationWindow()
        {
            InitializeComponent();
            InitializeLookupControls();
            DataContext = _member;
            TabItemContactInformation.DataContext = _member.ContactInformation;
        }

        public MemberInformationWindow(string memberCode)
        {
            InitializeComponent();
            //_member = Member.GetByCode(memberCode);
            _member = Nfmb.FindByCode(memberCode);

            InitializeLookupControls();
            DataContext = _member;
            TabItemContactInformation.DataContext = _member.ContactInformation;

            //UpdateAge(_member.Birthday);
            //lblMemberCodeName.Content = string.Format("{0} - {1}", _member.MemberCode, _member.MemberName);

            _member.ContactInformation = Contact.WhereMemberCodeIs(memberCode);

            imgPhoto.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Picture);
            imgSignature.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Signature);

            CrudButtons.Visibility = Visibility.Hidden;
            stbMemberNameCode.IsEnabled = false;
        }

        #region --- | Create | Destroy | Read | Update | ---

        private void Create(object sender, RoutedEventArgs e)
        {
            DataContext = _member = new Nfmb();
            ShowValidateMemberWindow(this, null);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (_member == null) return;
            if (_member.ID <= 0 ) return;

            const string confirmMessage = "You are about to delete current Member information. Do you want to proceed?";
            if (MessageWindow.ShowConfirmMessage(confirmMessage) != MessageBoxResult.Yes) return;

            _member.Destroy();
            MessageWindow.ShowNotifyMessage("Member information deleted!");
            DataContext = _member = new Nfmb();
        }

        private void Read(object sender, RoutedEventArgs e)
        {
            var members = Nfmb.GetList();
            var searchItems =
                members.Select(
                    member =>
                    new SearchItem(member.ID, member.MemberName){ItemCode = member.MemberCode}).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true) return;

            _member.Find(searchByCodeWindow.SelectedItem.ItemId);
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            DataContext = _member;
            //lblMemberCodeName.Content = string.Format("{0} - {1}", _member.MemberCode, _member.MemberName);

            //_member.ContactInformation = Contact.WhereMemberCodeIs(_member.MemberCode);
            //UpdateAge(_member.Birthday);
            TabItemContactInformation.DataContext = _member.ContactInformation;

            imgPhoto.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Picture);
            imgSignature.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Signature);
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_member.MemberName))
            {
                MessageWindow.ShowAlertMessage("Member Name must not be empty!");
                return;
            }
            
            var result = _member.ID == 0 ? _member.Create() : _member.Update();

            if (result.Success)
            {
                if (_member.ContactInformation.ID > 0)
                {
                    _member.ContactInformation.Update();
                }
                else
                {
                    _member.ContactInformation.Create();
                }
                //lblMemberCodeName.Content = string.Format("{0} - {1}", _member.MemberCode, _member.MemberName);
                _member.Find(_member.ID);
                RefreshDisplay();           
                MessageWindow.ShowNotifyMessage("Member information saved!");
            }
            else
                MessageWindow.ShowAlertMessage("Unable to save Member information! " + result.Message);
        }

        #endregion --- | Create | Destroy | Read | Update | ---

        private void btnBrowsePhoto_Click(object sender, RoutedEventArgs e) {
            var openFileDialog = new OpenFileDialog {
                CheckFileExists = true,
                Filter = string.Format("Image Files (*)|*.bmp;*.gif;*.jpg"),
                Title = String.Format("Picture Browser")
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            _member.ContactInformation.Picture = ImageTool.GetBytesFromImageFile(openFileDialog.FileName);
            imgPhoto.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Picture);
        }

        private void btnBrowseSignature_Click(object sender, RoutedEventArgs e) {
            var openFileDialog = new OpenFileDialog {
                CheckFileExists = true,
                Filter = string.Format("Image Files (*)|*.bmp;*.gif;*.jpg"),
                Title = String.Format("Signature Browser")
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            _member.ContactInformation.Signature = ImageTool.GetBytesFromImageFile(openFileDialog.FileName);
            imgSignature.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Signature);
        }

        private void InitializeLookupControls()
        {
            List<MembershipType> merbershipTypes = MembershipType.GetList();
            cboMembershipType.Items.Clear();
            foreach (MembershipType item in merbershipTypes)
            {
                cboMembershipType.Items.Add(item.Description);
            }

            List<Classification> classifications = Classification.GetList();
            cboClassification.Items.Clear();
            foreach (Classification item in classifications)
            {
                cboClassification.Items.Add(item.Description);
            }

            List<Area> locations = Area.GetList();
            cboArea.Items.Clear();
            foreach (Area item in locations)
            {
                cboArea.Items.Add(item.AreaName);
            }

            List<Collector> collectors = Collector.GetList();
            cboCollector.Items.Clear();
            foreach (Collector item in collectors)
            {
                cboCollector.Items.Add(item.CollectorName);
            }

            List<Branch> branches = Branch.GetList();
            cboBranch.Items.Clear();
            foreach (Branch item in branches)
            {
                cboBranch.Items.Add(item.BranchName);
            }

            List<Department> departments = Department.GetList();
            cboDepartment.Items.Clear();
            foreach (Department item in departments)
            {
                cboDepartment.Items.Add(item.DepartmentName);
            }
        }

        private void ShowValidateMemberWindow(object sender, RoutedEventArgs e)
        {
            //string tempFile = Path.GetTempFileName().Replace(".tmp", ".xml");
            //var serializer = new XmlSerializer(typeof (Nfmb));
            //var xmlWriterSettings = new XmlWriterSettings {Indent = true};
            //using (XmlWriter xmlWriter = XmlWriter.Create(tempFile, xmlWriterSettings))
            //{
            //    serializer.Serialize(xmlWriter, _member);
            //}

            //Nfmb modifiedMember;
            //using (XmlReader xmlReader = XmlReader.Create(tempFile))
            //{
            //    modifiedMember = (Nfmb)serializer.Deserialize(xmlReader);
            //}
            //File.Delete(tempFile);
            //var memberNameSetupWindow = new MemberValidationWindow(modifiedMember);

            var biometrics = new Contact();
            if (_member.ContactInformation != null)
            {
                biometrics = _member.ContactInformation;
            }
            else
            {
                biometrics.MemberCode = _member.MemberCode;
                biometrics.MemberName = _member.MemberName;
            }

            var manipulationMode = Controllers.ModelController.DataManipulationType.Create;
            if(_member.ID > 0)
            {
                manipulationMode = Controllers.ModelController.DataManipulationType.Update;
            }
            var memberNameSetupWindow = new MemberValidationWindow(biometrics, manipulationMode);
            memberNameSetupWindow.ShowDialog();
            if (memberNameSetupWindow.DialogResult != true) return;
            _member.ContactInformation = memberNameSetupWindow.ValidatedBiometrics;
            _member.MemberName = _member.ContactInformation.MemberName;
            _member.MemberCode = _member.ContactInformation.MemberCode;
            //DataContext = _member;
        }

        //private void UpdateAge(DateTime birthdate)
        //{
        //    var presentBirthDate = new DateTime(DateTime.Now.Year, birthdate.Month, birthdate.Day);
        //    int age = DateTime.Now.Year - birthdate.Year;
        //    if (presentBirthDate > DateTime.Now)
        //    {
        //        age -= 1;
        //    }
        //    _member.Age = age;
        //    //txtAge.Text = age.ToString(CultureInfo.InvariantCulture);
        //}
    }
}
