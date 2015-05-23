using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Text;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Utilities
{
    public static class Converter
    {

        #region --- Convert An Amount to Words ---

        public static string AmountToWords(decimal number, string currency = "Peso")
        {
            decimal originalNumber = number;

            //var converter = new Converter();

            string numberToWords = string.Empty;

            decimal wholeNumber = Math.Truncate(number);
            short decimalNumber = Convert.ToInt16((number - wholeNumber)*100);

            const decimal trilPlace = 1000000000000L;
            const decimal billPlace = 1000000000;
            const decimal millPlace = 1000000;
            const decimal thouPlace = 1000;
            const decimal hundPlace = 100;

            if (number == 0)
            {
                return "Zero";
            }

            if (number < 0)
            {
                number = Math.Abs(number);
            }

            decimal maxNumber = (trilPlace*1000) - Convert.ToDecimal(0.01);

            if (number > maxNumber)
            {
                numberToWords = string.Format("Error: Number to high. Maximum is {0:N2}", maxNumber);
                return numberToWords;
            }

            if (wholeNumber >= trilPlace)
            {
                var intNumber = (int) Math.Floor(wholeNumber/trilPlace);
                numberToWords += string.Format("{0} {1} ", GetHundredsToWords(intNumber), "Trillion");
                wholeNumber -= intNumber*trilPlace;
            }

            if (wholeNumber >= billPlace)
            {
                var intNumber = (int) Math.Floor(wholeNumber/billPlace);
                numberToWords += string.Format("{0} {1} ", GetHundredsToWords(intNumber), "Billion");
                wholeNumber -= intNumber*billPlace;
            }

            if (wholeNumber >= millPlace)
            {
                var intNumber = (int) Math.Floor(wholeNumber/millPlace);
                numberToWords += string.Format("{0} {1} ", GetHundredsToWords(intNumber), "Million");
                wholeNumber -= intNumber*millPlace;
            }

            if (wholeNumber >= thouPlace)
            {
                var intNumber = (int) Math.Floor(wholeNumber/thouPlace);
                numberToWords += string.Format("{0} {1} ", GetHundredsToWords(intNumber), "Thousand");
                wholeNumber -= intNumber*thouPlace;
            }

            if (wholeNumber >= hundPlace)
            {
                var intNumber = (int) wholeNumber;
                numberToWords += GetHundredsToWords(intNumber);
                wholeNumber -= intNumber;
            }

            //-- this will only be ran if original number is less than 100
            if (wholeNumber < 100 & wholeNumber > 0)
            {
                var intNumber = (int) wholeNumber;
                numberToWords += GetTensToWords(intNumber);
            }

            if (originalNumber > 1)
            {
                if (!currency.EndsWith("s"))
                {
                    currency += "s";
                }
            }
            numberToWords = string.Format("{0} {1}", numberToWords, currency);

            if (decimalNumber > 0)
            {
                string centavo = "Cents";
                if (decimalNumber == 1)
                {
                    centavo = "Cent";
                }
                numberToWords += string.Format(" and {0} {1}", GetTensToWords(decimalNumber), centavo);
            }

            while (numberToWords.Contains("  "))
            {
                numberToWords = numberToWords.Replace("  ", " ");
            }

            return numberToWords;
        }

        private static string GetHundredsToWords(int number)
        {
            string hundredsToWords = string.Empty;
            if (number >= 100)
            {
                var hundred = (int) Math.Floor(number/100m);
                hundredsToWords = string.Format("{0} {1}", GetTeensToWords(hundred), "Hundred");
                number -= hundred*100;
            }
            if (number > 0)
            {
                hundredsToWords = string.Format("{0} {1}", hundredsToWords, GetTensToWords(number));
            }
            return hundredsToWords;
        }

        private static string GetTensToWords(int number)
        {
            string tensToWords = string.Empty;
            var arrayOfTens = new Dictionary<int, string>
                {
                    {20, "Twenty"},
                    {30, "Thirty"},
                    {40, "Forty"},
                    {50, "Fifty"},
                    {60, "Sixty"},
                    {70, "Seventy"},
                    {80, "Eighty"},
                    {90, "Ninety"}
                };

            int ten = 0;
            if (number >= 20)
            {
                ten = (int) Math.Floor(number/10m);
                tensToWords = arrayOfTens[ten*10];
                number -= ten*10;
            }
            if (number > 0)
            {
                tensToWords = ten > 0
                                  ? string.Format("{0} {1}", tensToWords, GetTeensToWords(number))
                                  : GetTeensToWords(number);
            }

            return tensToWords;
        }

        private static string GetTeensToWords(int number)
        {
            var arrayOfTeens = new Dictionary<int, string>
                {
                    {1, "One"},
                    {2, "Two"},
                    {3, "Three"},
                    {4, "Four"},
                    {5, "Five"},
                    {6, "Six"},
                    {7, "Seven"},
                    {8, "Eight"},
                    {9, "Nine"},
                    {10, "Ten"},
                    {11, "Eleven"},
                    {12, "Twelve"},
                    {13, "Thirteen"},
                    {14, "Fourteen"},
                    {15, "Fifteen"},
                    {16, "Sixteen"},
                    {17, "Seventeen"},
                    {18, "Eighteen"},
                    {19, "Nineteen"}
                };

            string teensToWords = arrayOfTeens[number];
            return teensToWords;
        }

        #endregion

        #region --- Convert A DateTime to MySQL Date Format

        public static string DateTimeToMySqlDate(DateTime datetime)
        {
            return string.Format("{0:yyyy-MM-dd}", datetime);
        }
        #endregion

        #region --- Convert A List to DataTable
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
#endregion 


        public static object ToNullIfDefault(object param)
        {
            if(param is string)
            {
                var value = Convert.ToString(param);
                if (value.Length == 0) return null;
                return param;
            }
            if (param is DateTime)
            {
                var value = Convert.ToDateTime(param);
                if (value == new DateTime()) return null;
                return param;
            }
            if (param is int)
            {
                var value = Convert.ToInt32(param);
                if (value == 0) return null;
                return param;
            }
            if (param is decimal)
            {
                var value = Convert.ToDecimal(param);
                if (value == 0) return null;
                return param;
            }

            return param;
        }

    }

    public static class DataConverter
    {

        internal static string ToUtf8String(object param)
        {
            return param == DBNull.Value ? string.Empty : Encoding.UTF8.GetString(ToByteArray(param));
        }

        internal static bool ToBoolean(object param)
        {
            return param != DBNull.Value && Convert.ToBoolean(param);
        }

        internal static string ToString(object param)
        {
            return param == DBNull.Value ? string.Empty : Convert.ToString(param);
        }

        internal static decimal ToDecimal(object param)
        {
            return param == DBNull.Value ? 0m : Convert.ToDecimal(param);
        }

        internal static DateTime ToDateTime(object param)
        {
            return param == DBNull.Value ? new DateTime() : Convert.ToDateTime(param);
        }

        internal static DateTime? ToNullableDateTime(object param)
        {
            return param == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(param);
        }

        internal static int ToInteger(object param)
        {
            return param == DBNull.Value ? 0 : Convert.ToInt32(param);
        }

        public static char ToCharacter(object param)
        {
            return param == DBNull.Value ? ' ' : Convert.ToChar(param);
        }

        public static ModeOfPayments ToModeOfPayment(object param)
        {
            var value = ToString(param);
            if (value.Length < 1)
            {
                return ModeOfPayments.NotSpecified;
            }

            var initial = value.Substring(0, 1);
            switch (initial)
            {
                case "D":
                case "d":
                    return ModeOfPayments.Daily;

                case "W":
                case "w":
                    return ModeOfPayments.Weekly;

                case "S":
                case "s":
                    return ModeOfPayments.SemiMonthly;

                case "M":
                case "m":     
                    return ModeOfPayments.Monthly;

                default:
                    return ModeOfPayments.NotSpecified;
            }
        }

        public static VoucherTypes ToVoucherType(object param)
        {
            var docType = ToString(param);
            switch (docType)
            {
                case "CV":
                case "cv":
                    return VoucherTypes.CV;

                case "JV":
                case "jv":
                    return VoucherTypes.JV;

                case "OR":
                case "or":
                    return VoucherTypes.OR;

                case "BG":
                case "bg":
                    return VoucherTypes.BG;

                default:
                    return VoucherTypes.BG;
            }
        }

        public static byte[] ToByteArray(object param)
        {
            return param == DBNull.Value ? new byte[0] : (byte[]) (param);
        }

        public static string ToTermsMode(object terms, object termsMode)
        {
            var iterms = ToInteger(terms);
            var value = ToString(termsMode);
            if (value.Length < 1)
            {
                switch (iterms)
                {
                    case 3:
                    case 2:
                        return "Years";

                    case 1:
                        return "Year";

                    case 12:
                    case 6:
                        return "Months";

                    case 200:
                    case 100:
                    case 30:
                    case 15:
                        return "Days";

                    default:
                        if (iterms < (3*12))
                            return "Months";
                        if (iterms < 200)
                            return "Months";
                        break;
                }
                return "";
            }

            var initial = value.Substring(0, 1);
            switch (initial)
            {
                case "D":
                case "d":
                    return iterms > 1 ? "Days" : "Day";

                case "M":
                case "m":
                case "S":
                case "s":
                    return iterms > 1 ? "Months" : "Month";

                case "Y":
                case "y":
                    return iterms > 1 ? "Years" : "Year";

                default:
                    return "NotSpecified";
            }
        }
    }
}