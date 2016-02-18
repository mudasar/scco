using System;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class CashAndCheckBreakDown : INotifyPropertyChanged
    {
        private decimal _bankAmount1;
        private decimal _bankAmount2;
        private decimal _bankAmount3;
        private decimal _bankAmount4;
        private decimal _bankAmount5;

        private string _bankCheck1;
        private string _bankCheck2;
        private string _bankCheck3;
        private string _bankCheck4;
        private string _bankCheck5;

        private DateTime _bankDate1;
        private DateTime _bankDate2;
        private DateTime _bankDate3;
        private DateTime _bankDate4;
        private DateTime _bankDate5;

        private string _bankName1;
        private string _bankName2;
        private string _bankName3;
        private string _bankName4;
        private string _bankName5;

        private int _deno01;
        private int _deno02;
        private int _deno03;
        private int _deno04;
        private int _deno05;
        private int _deno06;
        private int _deno07;
        private int _deno08;
        private int _deno09;
        private int _deno10;

        private decimal _denoAmount01;
        private decimal _denoAmount02;
        private decimal _denoAmount03;
        private decimal _denoAmount04;
        private decimal _denoAmount05;
        private decimal _denoAmount06;
        private decimal _denoAmount07;
        private decimal _denoAmount08;
        private decimal _denoAmount09;
        private decimal _denoAmount10;
        private decimal _totalCashAmount;
        private decimal _totalCashAndCheckAmount;
        private decimal _totalCheckAmount;

        public decimal BankAmount1
        {
            get { return _bankAmount1; }
            set
            {
                _bankAmount1 = value;
                OnPropertyChanged("BankAmount1");

                UpdateTotalCheckAmount();
            }
        }

        public decimal BankAmount2
        {
            get { return _bankAmount2; }
            set
            {
                _bankAmount2 = value;
                OnPropertyChanged("BankAmount2");

                UpdateTotalCheckAmount();
            }
        }

        public decimal BankAmount3
        {
            get { return _bankAmount3; }
            set
            {
                _bankAmount3 = value;
                OnPropertyChanged("BankAmount3");

                UpdateTotalCheckAmount();
            }
        }

        public decimal BankAmount4
        {
            get { return _bankAmount4; }
            set
            {
                _bankAmount4 = value;
                OnPropertyChanged("BankAmount4");

                UpdateTotalCheckAmount();
            }
        }

        public decimal BankAmount5
        {
            get { return _bankAmount5; }
            set
            {
                _bankAmount5 = value;
                OnPropertyChanged("BankAmount5");

                UpdateTotalCheckAmount();
            }
        }

        public string BankCheck1
        {
            get { return _bankCheck1; }
            set
            {
                _bankCheck1 = value;
                OnPropertyChanged("BankCheck1");
            }
        }

        public string BankCheck2
        {
            get { return _bankCheck2; }
            set
            {
                _bankCheck2 = value;
                OnPropertyChanged("BankCheck2");
            }
        }

        public string BankCheck3
        {
            get { return _bankCheck3; }
            set
            {
                _bankCheck3 = value;
                OnPropertyChanged("BankCheck3");
            }
        }

        public string BankCheck4
        {
            get { return _bankCheck4; }
            set
            {
                _bankCheck4 = value;
                OnPropertyChanged("BankCheck4");
            }
        }

        public string BankCheck5
        {
            get { return _bankCheck5; }
            set
            {
                _bankCheck5 = value;
                OnPropertyChanged("BankCheck5");
            }
        }

        public DateTime BankDate1
        {
            get { return _bankDate1; }
            set
            {
                _bankDate1 = value;
                OnPropertyChanged("BankDate1");
            }
        }

        public DateTime BankDate2
        {
            get { return _bankDate2; }
            set
            {
                _bankDate2 = value;
                OnPropertyChanged("BankDate2");
            }
        }

        public DateTime BankDate3
        {
            get { return _bankDate3; }
            set
            {
                _bankDate3 = value;
                OnPropertyChanged("BankDate3");
            }
        }

        public DateTime BankDate4
        {
            get { return _bankDate4; }
            set
            {
                _bankDate4 = value;
                OnPropertyChanged("BankDate4");
            }
        }

        public DateTime BankDate5
        {
            get { return _bankDate5; }
            set
            {
                _bankDate5 = value;
                OnPropertyChanged("BankDate5");
            }
        }


        public string BankName1
        {
            get { return _bankName1; }
            set
            {
                _bankName1 = value;
                OnPropertyChanged("BankName1");
            }
        }

        public string BankName2
        {
            get { return _bankName2; }
            set
            {
                _bankName2 = value;
                OnPropertyChanged("BankName2");
            }
        }

        public string BankName3
        {
            get { return _bankName3; }
            set
            {
                _bankName3 = value;
                OnPropertyChanged("BankName3");
            }
        }

        public string BankName4
        {
            get { return _bankName4; }
            set
            {
                _bankName4 = value;
                OnPropertyChanged("BankName4");
            }
        }

        public string BankName5
        {
            get { return _bankName5; }
            set
            {
                _bankName5 = value;
                OnPropertyChanged("BankName5");
            }
        }


        public int Deno01
        {
            get { return _deno01; }
            set
            {
                _deno01 = value;
                OnPropertyChanged("Deno01");

                DenoAmount01 = value*1000m;
            }
        }

        public int Deno02
        {
            get { return _deno02; }
            set
            {
                _deno02 = value;
                OnPropertyChanged("Deno02");

                DenoAmount02 = value*500m;
            }
        }

        public int Deno03
        {
            get { return _deno03; }
            set
            {
                _deno03 = value;
                OnPropertyChanged("Deno03");

                DenoAmount03 = value*100m;
            }
        }

        public int Deno04
        {
            get { return _deno04; }
            set
            {
                _deno04 = value;
                OnPropertyChanged("Deno04");

                DenoAmount04 = value*50m;
            }
        }

        public int Deno05
        {
            get { return _deno05; }
            set
            {
                _deno05 = value;
                OnPropertyChanged("Deno05");

                DenoAmount05 = value*20m;
            }
        }

        public int Deno06
        {
            get { return _deno06; }
            set
            {
                _deno06 = value;
                OnPropertyChanged("Deno06");

                DenoAmount06 = value*10m;
            }
        }

        public int Deno07
        {
            get { return _deno07; }
            set
            {
                _deno07 = value;
                OnPropertyChanged("Deno07");

                DenoAmount07 = value*5m;
            }
        }

        public int Deno08
        {
            get { return _deno08; }
            set
            {
                _deno08 = value;
                OnPropertyChanged("Deno08");

                DenoAmount08 = value*1m;
            }
        }

        public int Deno09
        {
            get { return _deno09; }
            set
            {
                _deno09 = value;
                OnPropertyChanged("Deno09");

                DenoAmount09 = value*200m;
            }
        }

        public int Deno10
        {
            get { return _deno10; }
            set
            {
                _deno10 = value;
                OnPropertyChanged("Deno10");

                DenoAmount10 = value*.25m;
            }
        }

        public decimal DenoAmount01
        {
            get { return _denoAmount01; }
            set
            {
                _denoAmount01 = value;
                OnPropertyChanged("DenoAmount01");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount02
        {
            get { return _denoAmount02; }
            set
            {
                _denoAmount02 = value;
                OnPropertyChanged("DenoAmount02");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount03
        {
            get { return _denoAmount03; }
            set
            {
                _denoAmount03 = value;
                OnPropertyChanged("DenoAmount03");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount04
        {
            get { return _denoAmount04; }
            set
            {
                _denoAmount04 = value;
                OnPropertyChanged("DenoAmount04");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount05
        {
            get { return _denoAmount05; }
            set
            {
                _denoAmount05 = value;
                OnPropertyChanged("DenoAmount05");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount06
        {
            get { return _denoAmount06; }
            set
            {
                _denoAmount06 = value;
                OnPropertyChanged("DenoAmount06");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount07
        {
            get { return _denoAmount07; }
            set
            {
                _denoAmount07 = value;
                OnPropertyChanged("DenoAmount07");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount08
        {
            get { return _denoAmount08; }
            set
            {
                _denoAmount08 = value;
                OnPropertyChanged("DenoAmount08");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount09
        {
            get { return _denoAmount09; }
            set
            {
                _denoAmount09 = value;
                OnPropertyChanged("DenoAmount09");

                UpdateTotalCashAmount();
            }
        }

        public decimal DenoAmount10
        {
            get { return _denoAmount10; }
            set
            {
                _denoAmount10 = value;
                OnPropertyChanged("DenoAmount10");

                UpdateTotalCashAmount();
            }
        }


        public decimal TotalCashAmount
        {
            get { return _totalCashAmount; }
            set
            {
                _totalCashAmount = value;
                OnPropertyChanged("TotalCashAmount");
            }
        }

        public decimal TotalCheckAmount
        {
            get { return _totalCheckAmount; }
            set
            {
                _totalCheckAmount = value;
                OnPropertyChanged("TotalCheckAmount");
            }
        }

        public decimal TotalCashAndCheckAmount
        {
            get { return _totalCashAndCheckAmount; }
            set
            {
                _totalCashAndCheckAmount = value;
                OnPropertyChanged("TotalCashAndCheckAmount");
            }
        }

        public bool HasCheckDeposit
        {
            get { return _totalCheckAmount > 0; }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void Copy(CashAndCheckBreakDown model)
        {
            if (model == null) return;
            Deno01 = model.Deno01;
            Deno02 = model.Deno02;
            Deno03 = model.Deno03;
            Deno04 = model.Deno04;
            Deno05 = model.Deno05;
            Deno06 = model.Deno06;
            Deno07 = model.Deno07;
            Deno08 = model.Deno08;
            Deno09 = model.Deno09;
            Deno10 = model.Deno10;

            BankName1 = model.BankName1;
            BankName2 = model.BankName2;
            BankName3 = model.BankName3;
            BankName4 = model.BankName4;
            BankName5 = model.BankName5;

            BankDate1 = model.BankDate1;
            BankDate2 = model.BankDate2;
            BankDate3 = model.BankDate3;
            BankDate4 = model.BankDate4;
            BankDate5 = model.BankDate5;

            BankCheck1 = model.BankCheck1;
            BankCheck2 = model.BankCheck2;
            BankCheck3 = model.BankCheck3;
            BankCheck4 = model.BankCheck4;
            BankCheck5 = model.BankCheck5;

            BankAmount1 = model.BankAmount1;
            BankAmount2 = model.BankAmount2;
            BankAmount3 = model.BankAmount3;
            BankAmount4 = model.BankAmount4;
            BankAmount5 = model.BankAmount5;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateTotalCashAmount()
        {
            TotalCashAmount = DenoAmount01 +
                              DenoAmount02 +
                              DenoAmount03 +
                              DenoAmount04 +
                              DenoAmount05 +
                              DenoAmount06 +
                              DenoAmount07 +
                              DenoAmount08 +
                              DenoAmount09 +
                              DenoAmount10;

            TotalCashAndCheckAmount = TotalCashAmount + TotalCheckAmount;
        }

        private void UpdateTotalCheckAmount()
        {
            TotalCheckAmount = BankAmount1 +
                               BankAmount2 +
                               BankAmount3 +
                               BankAmount4 +
                               BankAmount5;

            TotalCashAndCheckAmount = TotalCashAmount + TotalCheckAmount;
        }

        internal static CashAndCheckBreakDown ExtractFromDataRow(DataRow dataRow)
        {
            var breakDown = new CashAndCheckBreakDown
                {
                    Deno01 = DataConverter.ToInteger(dataRow["DEN1"]),
                    Deno02 = DataConverter.ToInteger(dataRow["DEN2"]),
                    Deno03 = DataConverter.ToInteger(dataRow["DEN3"]),
                    Deno04 = DataConverter.ToInteger(dataRow["DEN4"]),
                    Deno05 = DataConverter.ToInteger(dataRow["DEN5"]),
                    Deno06 = DataConverter.ToInteger(dataRow["DEN6"]),
                    Deno07 = DataConverter.ToInteger(dataRow["DEN7"]),
                    Deno08 = DataConverter.ToInteger(dataRow["DEN8"]),
                    Deno09 = DataConverter.ToInteger(dataRow["DEN9"]),
                    Deno10 = DataConverter.ToInteger(dataRow["DEN10"]),
                    BankName1 = DataConverter.ToString(dataRow["BNAME1"]),
                    BankName2 = DataConverter.ToString(dataRow["BNAME2"]),
                    BankName3 = DataConverter.ToString(dataRow["BNAME3"]),
                    BankName4 = DataConverter.ToString(dataRow["BNAME4"]),
                    BankName5 = DataConverter.ToString(dataRow["BNAME5"]),
                    BankCheck1 = DataConverter.ToString(dataRow["BCHECK1"]),
                    BankCheck2 = DataConverter.ToString(dataRow["BCHECK2"]),
                    BankCheck3 = DataConverter.ToString(dataRow["BCHECK3"]),
                    BankCheck4 = DataConverter.ToString(dataRow["BCHECK4"]),
                    BankCheck5 = DataConverter.ToString(dataRow["BCHECK5"]),
                    BankDate1 = DataConverter.ToDateTime(dataRow["BDATE1"]),
                    BankDate2 = DataConverter.ToDateTime(dataRow["BDATE2"]),
                    BankDate3 = DataConverter.ToDateTime(dataRow["BDATE3"]),
                    BankDate4 = DataConverter.ToDateTime(dataRow["BDATE4"]),
                    BankDate5 = DataConverter.ToDateTime(dataRow["BDATE5"]),
                    BankAmount1 = DataConverter.ToDecimal(dataRow["BAMT1"]),
                    BankAmount2 = DataConverter.ToDecimal(dataRow["BAMT2"]),
                    BankAmount3 = DataConverter.ToDecimal(dataRow["BAMT3"]),
                    BankAmount4 = DataConverter.ToDecimal(dataRow["BAMT4"]),
                    BankAmount5 = DataConverter.ToDecimal(dataRow["BAMT5"])
                };

            return breakDown;
        }

        public static CashAndCheckBreakDown ExtractFromCashOnHand(OfficialReceipt cashOnHand)
        {
            var breakDown = new CashAndCheckBreakDown
                {
                    Deno01 = cashOnHand.Deno01,
                    Deno02 = cashOnHand.Deno02,
                    Deno03 = cashOnHand.Deno03,
                    Deno04 = cashOnHand.Deno04,
                    Deno05 = cashOnHand.Deno05,
                    Deno06 = cashOnHand.Deno06,
                    Deno07 = cashOnHand.Deno07,
                    Deno08 = cashOnHand.Deno08,
                    Deno09 = cashOnHand.Deno09,
                    Deno10 = cashOnHand.Deno10,
                    BankName1 = cashOnHand.BankName1,
                    BankName2 = cashOnHand.BankName2,
                    BankName3 = cashOnHand.BankName3,
                    BankName4 = cashOnHand.BankName4,
                    BankName5 = cashOnHand.BankName5,
                    BankCheck1 = cashOnHand.BankCheck1,
                    BankCheck2 = cashOnHand.BankCheck2,
                    BankCheck3 = cashOnHand.BankCheck3,
                    BankCheck4 = cashOnHand.BankCheck4,
                    BankCheck5 = cashOnHand.BankCheck5,
                    BankDate1 = cashOnHand.BankDate1,
                    BankDate2 = cashOnHand.BankDate2,
                    BankDate3 = cashOnHand.BankDate3,
                    BankDate4 = cashOnHand.BankDate4,
                    BankDate5 = cashOnHand.BankDate5,
                    BankAmount1 = cashOnHand.BankAmount1,
                    BankAmount2 = cashOnHand.BankAmount2,
                    BankAmount3 = cashOnHand.BankAmount3,
                    BankAmount4 = cashOnHand.BankAmount4,
                    BankAmount5 = cashOnHand.BankAmount5
                };

            return breakDown;
        }
    }
}