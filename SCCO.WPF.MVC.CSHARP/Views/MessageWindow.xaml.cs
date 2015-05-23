using System.Windows;
using System.Windows.Media;

namespace SCCO.WPF.MVC.CS.Views {
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow 
    {

        public MessageWindow(MessageBoxType messageBoxType)
        {
            InitializeComponent();

            imgAlert.Visibility = Visibility.Hidden;
            imgConfirm.Visibility = Visibility.Hidden;
            imgInfo.Visibility = Visibility.Hidden;
            imgWarning.Visibility = Visibility.Hidden;

            //-→ hide unnecessary buttons
            btnYes.Visibility = Visibility.Hidden;
            btnNo.Visibility = Visibility.Hidden;
            btnOK.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;

            lblFormTitle.Content = " ";

            switch (messageBoxType)
            {
                    case MessageBoxType.InformBox:
                    Canvass.Fill = new SolidColorBrush(Color.FromRgb(33, 98, 160));
                    Canvass.Stroke = new SolidColorBrush(Color.FromArgb(192, 33, 98, 160));
                    break;

                    case MessageBoxType.AlertBox:
                    Canvass.Fill = new SolidColorBrush(Color.FromRgb(255, 63, 0));
                    Canvass.Stroke = new SolidColorBrush(Color.FromArgb(192, 255, 63, 0));
                    //Canvass.Stroke = new SolidColorBrush(Color.FromArgb(192, 245, 141, 0));
                    break;

                    case MessageBoxType.ConfirmBox:
                    Canvass.Fill = new SolidColorBrush(Color.FromRgb(61, 16, 123));
                    Canvass.Stroke = new SolidColorBrush(Color.FromArgb(192, 61, 16, 123));
                    break;

                    case MessageBoxType.WarningBox:
                    Canvass.Fill = new SolidColorBrush(Color.FromRgb(154, 15, 15));
                    Canvass.Stroke = new SolidColorBrush(Color.FromArgb(192, 154, 15, 15));
                    break;
            }
        }

        public MessageBoxResult MessageBoxResult { get; set; }

        public enum MessageBoxType
        {
            AlertBox,
            ConfirmBox,
            WarningBox,
            InformBox,
        }

        public enum ResponseButton {
            Ok,
            OkCancel,
            Yes,
            YesNo
        }

        // Add any initialization after the InitializeComponent() call.
        private void RespondedWithYes(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Yes;
            Close();
        }

        private void RespondedWithNo(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.No;
            Close();
        }
        
        private void RespondedWithCancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Cancel;
            Close();
        }
        
        private void RespondedWithOk(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.OK;
            Close();
        }

        public static MessageBoxResult ShowAlertMessage(string alertMessage) {
            var messageWindow = new MessageWindow(MessageBoxType.AlertBox);
            const ResponseButton responseButton = ResponseButton.Ok;
            SetMessageButtons(messageWindow, responseButton);
            messageWindow.txtMessageString.Text = alertMessage;
            messageWindow.imgAlert.Visibility = Visibility.Visible;
            messageWindow.lblFormTitle.Content = "Alert";
            messageWindow.ShowDialog();
            return messageWindow.MessageBoxResult;
        }

        public static MessageBoxResult ShowConfirmMessage(string confirmMessage) {
            var messageWindow = new MessageWindow(MessageBoxType.ConfirmBox);
            const ResponseButton responseButton = ResponseButton.YesNo;
            SetMessageButtons(messageWindow, responseButton);
            messageWindow.txtMessageString.Text = confirmMessage;
            messageWindow.imgConfirm.Visibility = Visibility.Visible;
            messageWindow.lblFormTitle.Content = "Confirm";
            messageWindow.ShowDialog();
            return messageWindow.MessageBoxResult;
        }

        public static MessageBoxResult ShowWarningMessage(string warningMessage)
        {
            var messageWindow = new MessageWindow(MessageBoxType.WarningBox);
            const ResponseButton responseButton = ResponseButton.OkCancel;
            SetMessageButtons(messageWindow, responseButton);
            messageWindow.txtMessageString.Text = warningMessage;
            messageWindow.imgWarning.Visibility = Visibility.Visible;
            messageWindow.lblFormTitle.Content = "Warning";
            messageWindow.ShowDialog();
            return messageWindow.MessageBoxResult;
        }

        public static MessageBoxResult ShowNotifyMessage(string notifyMessage) {
            var messageWindow = new MessageWindow(MessageBoxType.InformBox);
            const ResponseButton responseButton = ResponseButton.Ok;
            SetMessageButtons(messageWindow, responseButton);
            messageWindow.txtMessageString.Text = notifyMessage;
            messageWindow.imgInfo.Visibility = Visibility.Visible;
            messageWindow.lblFormTitle.Content = "Notify";
            messageWindow.ShowDialog();
            return messageWindow.MessageBoxResult;
        }

        static void SetMessageButtons(MessageWindow messageWindow, ResponseButton responseButton)
        {
            switch (responseButton)
            {
                    case ResponseButton.Ok:
                    messageWindow.btnOK.Visibility = Visibility.Visible;
                    break;
                    case ResponseButton.OkCancel:
                    messageWindow.btnOK.Visibility = Visibility.Visible;
                    messageWindow.btnCancel.Visibility = Visibility.Visible;
                    break;

                    case ResponseButton.Yes:
                    messageWindow.btnYes.Visibility = Visibility.Visible;
                    break;

                    case ResponseButton.YesNo:
                    messageWindow.btnYes.Visibility = Visibility.Visible;
                    messageWindow.btnNo.Visibility = Visibility.Visible;
                    break;

            }
        }




        public static MessageBoxResult AlertRecordIsLocked()
        {
            return ShowAlertMessage(@"Record is locked.");
        }
        public static MessageBoxResult AlertDebitAndCreditNotEqual()
        {
            return ShowAlertMessage(@"Debit and Credit not equal.");
        }

        public static MessageBoxResult AlertAddNewRecordNotAllowed()
        {
            return ShowAlertMessage(@"You are not allowed to add new record. Please check your current Transaction Date setting.");
        }

        public static MessageBoxResult ConfirmSaveChangesFirst()
        {
            return ShowConfirmMessage(@"Do you want to save your changes first?");
        }
        public static MessageBoxResult ConfirmCancelVoucher()
        {
            return ShowConfirmMessage(@"Do you really want to cancel this voucher?");
        }
        public static MessageBoxResult ConfirmDeleteVoucher()
        {
            return ShowConfirmMessage(@"Do you really want to delete this voucher?");
        }

        public static MessageBoxResult ConfirmDeleteRecord()
        {
            return ShowConfirmMessage(@"Do you really want to delete this record?");
        }

        public static MessageBoxResult NotifyVoucherSaved()
        {
            return ShowNotifyMessage(@"Voucher information saved.");
        }


    }

}
