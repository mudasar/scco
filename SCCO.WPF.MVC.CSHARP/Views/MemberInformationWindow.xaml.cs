﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class MemberInformationWindow
    {
        private Nfmb _member = new Nfmb();

        public MemberInformationWindow()
        {
            InitializeComponent();
            InitializeLookupControls();
            DataContext = _member;
            TabItemContactInformation.DataContext = _member.ContactInformation;
            stbMemberNameCode.CustomTooltip = "Edit Member Name";
        }

        public MemberInformationWindow(string memberCode)
        {
            InitializeComponent();
            _member = Nfmb.FindByCode(memberCode);

            InitializeLookupControls();
            DataContext = _member;
            _member.ContactInformation = Contact.FindByMemberCode(memberCode);
            TabItemContactInformation.DataContext = _member.ContactInformation;

            imgPhoto.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Picture);
            imgSignature.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Signature);

            CrudButtons.Visibility = Visibility.Hidden;
            stbMemberNameCode.IsEnabled = false;
        }

        #region --- | Create | Destroy | Read | Update | ---

        private void Create(object sender, RoutedEventArgs e)
        {
            _member = new Nfmb {ContactInformation = new Contact()};
            RefreshDisplay();
            ShowValidateMemberWindow(this, null);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (_member == null) return;
            if (_member.ID <= 0) return;

            const string confirmMessage = "You are about to delete current Member information. Do you want to proceed?";
            if (MessageWindow.ShowConfirmMessage(confirmMessage) != MessageBoxResult.Yes) return;

            _member.Destroy();
            MessageWindow.ShowNotifyMessage("Member information deleted!");
            DataContext = _member = new Nfmb();
        }

        private void Read(object sender, RoutedEventArgs e)
        {
            List<Nfmb> members = Nfmb.GetList();
            List<SearchItem> searchItems =
                members.Select(
                    member =>
                    new SearchItem(member.ID, member.MemberName) {ItemCode = member.MemberCode}).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true) return;

            _member.Find(searchByCodeWindow.SelectedItem.ItemId);
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            DataContext = _member;
            _member.LoadContactInformation();
            RefreshContactionInformation();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_member.MemberName))
            {
                MessageWindow.ShowAlertMessage("Member Name must not be empty!");
                return;
            }

            Result result = _member.ID == 0 ? _member.Create() : _member.Update();

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
                _member.Find(_member.ID);
                RefreshDisplay();
                MessageWindow.ShowNotifyMessage("Member information saved!");
            }
            else
                MessageWindow.ShowAlertMessage("Unable to save Member information! " + result.Message);
        }

        #endregion --- | Create | Destroy | Read | Update | ---

        private void btnBrowsePhoto_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = string.Format("Image Files (*)|*.bmp;*.gif;*.jpg"),
                Title = String.Format("Picture Browser")
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            //_member.ContactInformation.Picture = ImageTool.GetBytesFromImageFile(openFileDialog.FileName);
            var imageFile = openFileDialog.FileName;
            imgPhoto.Source = ImageTool.CreateImageSourceFromBytes(ImageTool.GetBytesFromImageFile(imageFile));
            _member.ContactInformation.SavePicture(imageFile);
        }

        private void btnBrowseSignature_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = string.Format("Image Files (*)|*.bmp;*.gif;*.jpg"),
                Title = String.Format("Signature Browser")
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            //_member.ContactInformation.Signature = ImageTool.GetBytesFromImageFile(openFileDialog.FileName);
            var imageFile = openFileDialog.FileName;
            imgSignature.Source =
                ImageTool.CreateImageSourceFromBytes(ImageTool.GetBytesFromImageFile(openFileDialog.FileName));
            _member.ContactInformation.SaveSignature(imageFile);
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
            var biometrics = new Contact();
            if (_member.ContactInformation != null)
            {
                if (_member.ContactInformation.ID != 0)
                {
                    biometrics = _member.ContactInformation;
                }
            }
            else
            {
                biometrics.MemberCode = _member.MemberCode;
                biometrics.MemberName = _member.MemberName;
            }

            var manipulationMode = ModelController.DataManipulationType.Create;
            if (_member.ID > 0)
            {
                manipulationMode = ModelController.DataManipulationType.Update;
            }
            var memberNameSetupWindow = new MemberValidationWindow(biometrics, manipulationMode);
            memberNameSetupWindow.ShowDialog();
            if (memberNameSetupWindow.DialogResult != true) return;

            _member.ContactInformation = memberNameSetupWindow.ValidatedBiometrics;
            _member.MemberName = _member.ContactInformation.MemberName;
            _member.MemberCode = _member.ContactInformation.MemberCode;
            DataContext = _member;

            RefreshContactionInformation();
        }

        private void RefreshContactionInformation()
        {
            TabItemContactInformation.DataContext = _member.ContactInformation;
            imgPhoto.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Picture);
            imgSignature.Source = ImageTool.CreateImageSourceFromBytes(_member.ContactInformation.Signature);
        }

        private void Print(object sender, RoutedEventArgs e)
        {
            try
            {
                var sqlCommand = GetMemberQuery();
                var sqlParam = new Database.SqlParameter("?MEM_CODE", _member.MemberCode);
                var dataTable = Database.DatabaseController.ExecuteSelectQuery(sqlCommand, sqlParam);
                foreach (System.Data.DataRow dataRow in dataTable.Rows)
                {
                    dataRow["PICTURE"] = _member.ContactInformation.Picture;
                }

                dataTable.TableName = "nfmb";
                var dataSet = new System.Data.DataSet();
                dataSet.Tables.Add(dataTable);
                var ri = new ReportItem();
                ri.ReportFile = "member_information_sheet.rpt";
                ri.Title = "Member Information Sheet";
                ri.DataSource = dataSet;
                ri.LoadReport();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }

        private string GetMemberQuery()
        {
            return @"
SELECT
nfmb.ID,
nfmb.MEM_CODE,
nfmb.MEM_NAME,
IF(nfmb.MEMBER, 'Yes', 'No') as MEMBER,
nfmb.AREA_CODE,
nfmb.ADDRESS1,
nfmb.ADDRESS2,
nfmb.ADDRESS3,
nfmb.HOUSE_STAT,
contacts.TELEPHONE,
contacts.MOBILE_PHONE AS MOBI_PHONE,
contacts.BUSINESS_PHONE AS BUSI_PHONE,
contacts.EMAIL as EMAIL,
contacts.PICTURE,
nfmb.BIRTHDAY,
nfmb.BAGE,
nfmb.BIRT_PLACE,
nfmb.MEMBERSHIP,
nfmb.SEX,
nfmb.CIVIL,
nfmb.DEGREE,
nfmb.OCCUPATION,
nfmb.BUSINESS,
nfmb.INCOME1,
nfmb.INCOME2,
nfmb.SPOUSE_OCC,
nfmb.SPOUSE_BUS,
nfmb.SPOUS_INC1,
nfmb.SPOUS_INC2,
IF(nfmb.CLOSE_ACCT, 'Yes', 'No') as CLOSE_ACCT,
nfmb.CLOSE_DATE,
nfmb.BENEFIT,
nfmb.AGE,
nfmb.RELATION,
nfmb.BENEFIT1,
nfmb.AGE1,
nfmb.RELATION1,
nfmb.BENEFIT2,
nfmb.AGE2,
nfmb.RELATION2,
nfmb.MEM_TYPE,
IF(nfmb.DAMAYAN, 'Yes', 'No') as DAMAYAN,
nfmb.PMS,
nfmb.D_DATE,
nfmb.COLLECTOR,
nfmb.BRANCH,
nfmb.CLASS
FROM `nfmb`
LEFT JOIN contacts
on nfmb.MEM_CODE = contacts.MEM_CODE
WHERE nfmb.MEM_CODE = ?MEM_CODE
";
        }
    }
}