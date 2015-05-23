using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for MemberInformationReportWindow.xaml
    /// </summary>
    public partial class MemberInformationReportWindow : Window
    {
        public MemberInformationReportWindow()
        {
            InitializeComponent();

            listBox1.ItemsSource = GetReportList();
        }
      
        private void ShowReportOnClick(object sender, RoutedEventArgs e)
        {
            var selectedReport = (ReportItem)listBox1.SelectedItem;

            switch (selectedReport.Code)
            {
                case "GetMemberListByCode":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListByCode());
                    break;
                case "GetMemberListByName":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListByName());
                    break;
                case "GetMemberListByGender":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListByGender());
                    break;
                case "GetMemberListByCivilStatus":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListByCivilStatus());
                    break;
                case "GetMemberListByIsMember":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListByIsMember());
                    break;
                case "GetMemberListByMembershipType":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListByMembershipType());
                    break;
                case "GetMemberListWhereIsDamayanMember":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListWhereIsDamayanMember());
                    break;
                case "GetMemberListByAge":
                    Controllers.ReportController.GenerateMemberListReport(Models.Member.GetMemberListByAge());
                    break;
            }
        }



        #region --- REPORT LIST ---

        private static IEnumerable<ReportItem> GetReportList()
        {
            var reportList = new List<ReportItem>
                                 {
                                     new ReportItem("GetMemberListByCode", "Member List By Code"),
                                     new ReportItem("GetMemberListByName", "Member List By Name"),
                                     new ReportItem("GetMemberListByGender", "Member List By Gender"),
                                     new ReportItem("GetMemberListByCivilStatus", "Member List By Civil Status"),
                                     new ReportItem("GetMemberListByIsMember", "Member List (Member and Non-Member)"),
                                     new ReportItem("GetMemberListByMembershipType", "Member List By Membership Type"),
                                     new ReportItem("GetMemberListWhereIsDamayanMember", "Damayan Member (PTTK)"),
                                     new ReportItem("GetMemberListByAge", "Member List By Age")
                                 };


            return reportList;
        }

        #endregion --- REPORT LIST ---

        internal struct ReportItem
        {
            public string Code { get; set; }
            public string Title { get; set; }

            public ReportItem(string code, string title)
                : this()
            {
                Code = code;
                Title = title;
            }

            public override string ToString()
            {
                return Title;
            }
        }
    }
}
