using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public class TimeDepositViewModel : INotifyPropertyChanged
    {
        private TimeDepositProducts _products;
        private TimeDepositProduct _selectedItem;
        private List<TermRange> _ranges;
        private DateTime _dateIn;
        private DateTime _dateMaturity;
        private TermRange _selectedTerm;
        private decimal _amount;
        private string _certificateNo;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public TimeDepositProducts Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products");}
        }

        public TimeDepositProduct SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");

                var ranges = new List<TermRange>();
                for (int i = _selectedItem.MinimumTerm; i <= _selectedItem.MaximumTerm; i++)
                {
                    ranges.Add(new TermRange(i));
                }
                Ranges = ranges;
                Amount = _selectedItem.MinimumAmount;
            }
        }

        public DateTime DateIn
        {
            get { return _dateIn; }
            set { _dateIn = value; OnPropertyChanged("DateIn"); }
        }

        public DateTime DateMaturity
        {
            get { return _dateMaturity; }
            set { _dateMaturity = value; OnPropertyChanged("DateMaturity"); }
        }

        public TermRange SelectedTerm
        {
            get { return _selectedTerm; }
            set
            {
                if (_selectedTerm == value) return;
                _selectedTerm = value; OnPropertyChanged("SelectedTerm");
                var dateIn = new DateTime(DateIn.Year, DateIn.Month, DateIn.Day);
                DateMaturity = dateIn.AddMonths(_selectedTerm.Value);
            }
        }

        public List<TermRange> Ranges
        {
            get { return _ranges; }
            set { _ranges = value; OnPropertyChanged("Ranges"); }
        }

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged("Amount"); }
        }

        public string CertificateNo
        {
            get { return _certificateNo; }
            set { _certificateNo = value; OnPropertyChanged("CertificateNo"); }
        }

        public TimeDepositDetails GenerateTimeDepositDetails()
        {
            var timeDepositDetails = new TimeDepositDetails();
            timeDepositDetails.Amount = Amount;
            timeDepositDetails.CertificateNo = CertificateNo;
            timeDepositDetails.DateIn = DateIn;
            timeDepositDetails.Maturity = DateMaturity;
            timeDepositDetails.Rate = SelectedItem.InterestRate;
            timeDepositDetails.Term = SelectedTerm.Value;
            timeDepositDetails.TermsMode = "Month";
            return timeDepositDetails;
        }
    }


    public class TimeDepositProducts : ObservableCollection<TimeDepositProduct>
    {
    }

    public class TermRange
    {
        public int Value;
        public string Name;

        public TermRange(int value)
        {
            Value = value;
            if (value%12 == 0)
            {
                int year = value/12;
                Name = string.Format("{0} {1}", year, Pluralize(year, "year"));
            }
            else
            {
                Name = string.Format("{0} {1}", value, Pluralize(value, "month"));
            }
        }

        public override string ToString()
        {
            return Name;
        }

        private static string Pluralize(int value, string text)
        {
            if (value > 1) return text + "s";
            return text;
        }
    }
}
