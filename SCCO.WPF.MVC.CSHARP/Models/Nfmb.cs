using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Nfmb : INotifyPropertyChanged, IModel
    {
        private int _id;
        private string _memberCode;
        private string _memberName;
        private bool _isMember;
        private string _areaCode;
        private string _address1;
        private string _address2;
        private string _address3;
        private string _homeOwnership;
        private string _telephone;
        private DateTime _birthday;
        private int _age;
        private string _birthPlace;
        private DateTime _membershipDate;
        private string _sex;
        private string _civilStatus;
        private string _educationalAttainment;
        private string _occupation;
        private string _business;
        private decimal _occupationIncome;
        private decimal _businessIncome;
        private string _spouseOccupation;
        private string _spouseBusiness;
        private decimal _spouseOccupationIncome;
        private decimal _spouseBusinessIncome;
        private bool _isAccountClosed;
        private DateTime _accountClosedSince;
        private string _beneficiary1Name;
        private int _beneficiary1Age;
        private string _beneficiary1Relationship;
        private string _beneficiary2Name;
        private int _beneficiary2Age;
        private string _beneficiary2Relationship;
        private bool _isLetterSend;
        private string _properName;
        private string _shareCapitalReceiptNo;
        private DateTime _shareCapitalDepositDate;
        private decimal _shareCapitalAmount;
        private bool _isDamayanMember;
        private DateTime _preMembershipSeminarDate;
        private DateTime _damayanMemberSince;
        private string _collectorName;
        private string _branchName;
        private string _department;
        private string _classification;
        private int _stubNo;
        private int _precintNo;
        private string _beneficiary3Relationship;
        private string _beneficiary3Name;
        private int _beneficiary3Age;
        private string _membershipType;
        private string _mobilePhone;
        private string _businessPhone;
        private string _email;
        private Contact _contactInformation;


        private List<SqlParameter> SqlParameters
        {
            get
            {
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?MEM_CODE", MemberCode);
                ModelController.AddParameter(sqlParameters, "?MEM_NAME", MemberName);
                ModelController.AddParameter(sqlParameters, "?MEMBER", IsMember);
                ModelController.AddParameter(sqlParameters, "?AREA_CODE", AreaCode);
                ModelController.AddParameter(sqlParameters, "?ADDRESS1", Address1);
                ModelController.AddParameter(sqlParameters, "?ADDRESS2", Address2);
                ModelController.AddParameter(sqlParameters, "?ADDRESS3", Address3);
                ModelController.AddParameter(sqlParameters, "?HOUSE_STAT", HomeOwnership);
                ModelController.AddParameter(sqlParameters, "?TELEPHONE", Telephone);
                ModelController.AddParameter(sqlParameters, "?BIRTHDAY", Birthday);

                ModelController.AddParameter(sqlParameters, "?BAGE", Age);
                ModelController.AddParameter(sqlParameters, "?BIRT_PLACE", BirthPlace);
                ModelController.AddParameter(sqlParameters, "?MEMBERSHIP", MembershipDate);

                ModelController.AddParameter(sqlParameters, "?SEX", Sex);
                ModelController.AddParameter(sqlParameters, "?CIVIL", CivilStatus);
                ModelController.AddParameter(sqlParameters, "?DEGREE", EducationalAttainment);
                ModelController.AddParameter(sqlParameters, "?OCCUPATION", Occupation);
                ModelController.AddParameter(sqlParameters, "?BUSINESS", Business);
                ModelController.AddParameter(sqlParameters, "?INCOME1", OccupationIncome);
                ModelController.AddParameter(sqlParameters, "?INCOME2", BusinessIncome);
                ModelController.AddParameter(sqlParameters, "?SPOUSE_OCC", SpouseOccupation);
                ModelController.AddParameter(sqlParameters, "?SPOUSE_BUS", SpouseBusiness);
                ModelController.AddParameter(sqlParameters, "?SPOUS_INC1", SpouseOccupationIncome);
                ModelController.AddParameter(sqlParameters, "?SPOUS_INC2", SpouseBusinessIncome);
                ModelController.AddParameter(sqlParameters, "?CLOSE_ACCT", IsAccountClosed);
                ModelController.AddParameter(sqlParameters, "?CLOSE_DATE", AccountClosedSince);
                ModelController.AddParameter(sqlParameters, "?BENEFIT", Beneficiary1Name);
                ModelController.AddParameter(sqlParameters, "?AGE", Beneficiary1Age);
                ModelController.AddParameter(sqlParameters, "?RELATION", Beneficiary1Relationship);

                ModelController.AddParameter(sqlParameters, "?BENEFIT1", Beneficiary2Name);
                ModelController.AddParameter(sqlParameters, "?AGE1", Beneficiary2Age);
                ModelController.AddParameter(sqlParameters, "?RELATION1", Beneficiary2Relationship);

                ModelController.AddParameter(sqlParameters, "?BENEFIT2", Beneficiary3Name);
                ModelController.AddParameter(sqlParameters, "?AGE2", Beneficiary3Age);
                ModelController.AddParameter(sqlParameters, "?RELATION2", Beneficiary3Relationship);

                ModelController.AddParameter(sqlParameters, "?MEM_TYPE", MembershipType);
                ModelController.AddParameter(sqlParameters, "?LETTER", IsLetterSend);
                ModelController.AddParameter(sqlParameters, "?NAME1", ProperName);
                ModelController.AddParameter(sqlParameters, "?OR_NO", ShareCapitalReceiptNo);
                ModelController.AddParameter(sqlParameters, "?DATED", ShareCapitalDepositDate);
                ModelController.AddParameter(sqlParameters, "?SC_AMT", ShareCapitalAmount);
                ModelController.AddParameter(sqlParameters, "?DAMAYAN", IsDamayanMember);
                ModelController.AddParameter(sqlParameters, "?PMS", PreMembershipSeminarDate);
                ModelController.AddParameter(sqlParameters, "?D_DATE", DamayanMemberSince);
                ModelController.AddParameter(sqlParameters, "?COLLECTOR", CollectorName);
                ModelController.AddParameter(sqlParameters, "?BRANCH", BranchName);
                ModelController.AddParameter(sqlParameters, "?DEPART", Department);
                ModelController.AddParameter(sqlParameters, "?CLASS", Classification);
                ModelController.AddParameter(sqlParameters, "?STUB_NO", StubNo);
                ModelController.AddParameter(sqlParameters, "?PRECINT", PrecintNo);

                return sqlParameters;
            }
        }

        private const string TABLE_NAME = "nfmb";

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string MemberCode //MEM_CODE
        {
            get { return _memberCode; }
            set { _memberCode = value; OnPropertyChanged("MemberCode");}
        }

        public string MemberName //MEM_NAME
        {
            get { return _memberName; }
            set { _memberName = value; OnPropertyChanged("MemberName");}
        }

        public bool IsMember //MEMBER
        {
            get { return _isMember; }
            set { _isMember = value; OnPropertyChanged("IsMember");}
        }

        public string AreaCode //AREA_CODE
        {
            get { return _areaCode; }
            set { _areaCode = value; OnPropertyChanged("AreaCode");}
        }

        public string Address1 //ADDRESS1
        {
            get { return _address1; }
            set { _address1 = value; OnPropertyChanged("Address1");}
        }

        public string Address2 //ADDRESS2
        {
            get { return _address2; }
            set { _address2 = value; OnPropertyChanged("Address2");}
        }

        public string Address3 //ADDRESS3
        {
            get { return _address3; }
            set { _address3 = value; OnPropertyChanged("Address3");}
        }

        public string HomeOwnership //HOUSE_STAT
        {
            get { return _homeOwnership; }
            set { _homeOwnership = value; OnPropertyChanged("HomeOwnership");}
        }

        public string Telephone //TELEPHONE
        {
            get { return _telephone; }
            set { _telephone = value; OnPropertyChanged("Telephone");}
        }

        public string MobilePhone //MOBI_PHONE
        {
            get { return _mobilePhone; }
            set { _mobilePhone = value; OnPropertyChanged("MobilePhone"); }
        }

        public string BusinessPhone //BUSI_PHONE
        {
            get { return _businessPhone; }
            set { _businessPhone = value; OnPropertyChanged("BusinessPhone"); }
        }

        public string Email //EMAIL
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }

        public DateTime Birthday //BIRTHDAY
        {
            get { return _birthday; }
            set { _birthday = value; OnPropertyChanged("Birthday");
                UpdateAge();
            }
        }

        public int Age //BAGE
        {
            get { return _age; }
            set { _age = value; OnPropertyChanged("Age");}
        }

        public string BirthPlace //BIRT_PLACE
        {
            get { return _birthPlace; }
            set { _birthPlace = value; OnPropertyChanged("BirthPlace");}
        }

        public DateTime MembershipDate //MEMBERSHIP
        {
            get { return _membershipDate; }
            set { _membershipDate = value; OnPropertyChanged("MembershipDate");}
        }

        public string Sex //SEX
        {
            get { return _sex; }
            set { _sex = value; OnPropertyChanged("Sex"); }
        }

        public string CivilStatus //CIVIL
        {
            get { return _civilStatus; }
            set { _civilStatus = value; OnPropertyChanged("CivilStatus");}
        }

        public string EducationalAttainment //DEGREE
        {
            get { return _educationalAttainment; }
            set { _educationalAttainment = value; OnPropertyChanged("EducationalAttainment"); }
        }

        public string Occupation //OCCUPATION
        {
            get { return _occupation; }
            set { _occupation = value; OnPropertyChanged("Occupation");}
        }

        public string Business //BUSINESS
        {
            get { return _business; }
            set { _business = value; OnPropertyChanged("Business");}
        }

        public decimal OccupationIncome //INCOME1
        {
            get { return _occupationIncome; }
            set { _occupationIncome = value; OnPropertyChanged("OccupationIncome");}
        }

        public decimal BusinessIncome //INCOME2
        {
            get { return _businessIncome; }
            set { _businessIncome = value; OnPropertyChanged("BusinessIncome");}
        }

        public string SpouseOccupation //SPOUSE_OCC
        {
            get { return _spouseOccupation; }
            set { _spouseOccupation = value; OnPropertyChanged("SpouseOccupation");}
        }

        public string SpouseBusiness //SPOUSE_BUS
        {
            get { return _spouseBusiness; }
            set { _spouseBusiness = value;OnPropertyChanged(SpouseBusiness); }
        }

        public decimal SpouseOccupationIncome //SPOUS_INC1
        {
            get { return _spouseOccupationIncome; }
            set { _spouseOccupationIncome = value;OnPropertyChanged("SpouseOccupationIncome"); }
        }

        public decimal SpouseBusinessIncome //SPOUS_INC2
        {
            get { return _spouseBusinessIncome; }
            set { _spouseBusinessIncome = value; OnPropertyChanged("SpouseBusinessIncome");}
        }

        public bool IsAccountClosed //CLOSE_ACCT
        {
            get { return _isAccountClosed; }
            set { _isAccountClosed = value; OnPropertyChanged("IsAccountClosed");}
        }

        public DateTime AccountClosedSince //CLOSE_DATE
        {
            get { return _accountClosedSince; }
            set { _accountClosedSince = value; OnPropertyChanged("AccountClosedSince");}
        }

        public string Beneficiary1Name //BENEFIT
        {
            get { return _beneficiary1Name; }
            set { _beneficiary1Name = value; OnPropertyChanged("Beneficiary1Name");}
        }

        public int Beneficiary1Age //AGE
        {
            get { return _beneficiary1Age; }
            set { _beneficiary1Age = value;OnPropertyChanged("Beneficiary1Age"); }
        }

        public string Beneficiary1Relationship //RELATION
        {
            get { return _beneficiary1Relationship; }
            set { _beneficiary1Relationship = value; OnPropertyChanged("Beneficiary1Relationship");}
        }

        public string Beneficiary2Name //BENEFIT1
        {
            get { return _beneficiary2Name; }
            set { _beneficiary2Name = value; OnPropertyChanged("Beneficiary2Name");}
        }

        public int Beneficiary2Age //AGE1
        {
            get { return _beneficiary2Age; }
            set { _beneficiary2Age = value; OnPropertyChanged("Beneficiary2Age");}
        }

        public string Beneficiary2Relationship //RELATION1
        {
            get { return _beneficiary2Relationship; }
            set { _beneficiary2Relationship = value;OnPropertyChanged("Beneficiary2Relationship"); }
        }

        public string Beneficiary3Name //BENEFIT1
        {
            get { return _beneficiary3Name; }
            set { _beneficiary3Name = value; OnPropertyChanged("Beneficiary3Name"); }
        }

        public int Beneficiary3Age //AGE1
        {
            get { return _beneficiary3Age; }
            set { _beneficiary3Age = value; OnPropertyChanged("Beneficiary3Age"); }
        }

        public string Beneficiary3Relationship //RELATION1
        {
            get { return _beneficiary3Relationship; }
            set { _beneficiary3Relationship = value; OnPropertyChanged("Beneficiary3Relationship"); }
        }

        public string MembershipType
        {
            get { return _membershipType; }
            set { _membershipType = value; OnPropertyChanged("MembershipType");}
        }

        public bool IsLetterSend //LETTER
        {
            get { return _isLetterSend; }
            set { _isLetterSend = value; OnPropertyChanged("IsLetterSend");}
        }

        public string ProperName //NAME1
        {
            get { return _properName; }
            set { _properName = value; OnPropertyChanged("ProperName");}
        }

        public string ShareCapitalReceiptNo //OR_NO
        {
            get { return _shareCapitalReceiptNo; }
            set { _shareCapitalReceiptNo = value; OnPropertyChanged("ShareCapitalReceiptNo"); }
        }

        public DateTime ShareCapitalDepositDate //DATED
        {
            get { return _shareCapitalDepositDate; }
            set { _shareCapitalDepositDate = value; OnPropertyChanged("ShareCapitalDepositDate");}
        }

        public decimal ShareCapitalAmount //SC_AMT
        {
            get { return _shareCapitalAmount; }
            set { _shareCapitalAmount = value; OnPropertyChanged("ShareCapitalAmount");}
        }

        public bool IsDamayanMember //DAMAYAN
        {
            get { return _isDamayanMember; }
            set { _isDamayanMember = value; OnPropertyChanged("IsDamayanMember");}
        }

        public DateTime PreMembershipSeminarDate //PMS
        {
            get { return _preMembershipSeminarDate; }
            set { _preMembershipSeminarDate = value; OnPropertyChanged("PreMembershipSeminarDate");}
        }

        public DateTime DamayanMemberSince //D_DATE
        {
            get { return _damayanMemberSince; }
            set { _damayanMemberSince = value; OnPropertyChanged("DamayanMemberSince"); }
        }

        public string CollectorName //COLLECTOR
        {
            get { return _collectorName; }
            set { _collectorName = value; OnPropertyChanged("CollectorName");}
        }

        public string BranchName //BRANCH
        {
            get { return _branchName; }
            set { _branchName = value; OnPropertyChanged("BranchName");}
        }

        public string Department //DEPART
        {
            get { return _department; }
            set { _department = value; OnPropertyChanged("Department");}
        }

        public string Classification //CLASS
        {
            get { return _classification; }
            set { _classification = value; OnPropertyChanged("Classification");}
        }

        public int StubNo //STUB_NO
        {
            get { return _stubNo; }
            set { _stubNo = value;OnPropertyChanged("StubNo"); }
        }

        public int PrecintNo // PRECINT
        {
            get { return _precintNo; }
            set { _precintNo = value;OnPropertyChanged("PrecintNo"); }
        }

        public Contact ContactInformation
        {
            get { return _contactInformation; }
            set { _contactInformation = value; OnPropertyChanged("ContactInformation"); }
        }

        private void UpdateAge()
        {
            var birthdate = _birthday;
            var presentBirthDate = new DateTime(DateTime.Now.Year, birthdate.Month, birthdate.Day);
            int age = DateTime.Now.Year - birthdate.Year;
            if (presentBirthDate > DateTime.Now)
            {
                age -= 1;
            }
           Age = age;
        }

        public string CompleteAddress()
        {
            var addressBuilder = new StringBuilder();
            var addresses = new List<string>();
            if (!string.IsNullOrEmpty(Address1))
            {
                addresses.Add(Address1);
            }
            if (!string.IsNullOrEmpty(Address2))
            {
                addresses.Add(Address2);
            }
            if (!string.IsNullOrEmpty(Address3))
            {
                addresses.Add(Address3);
            }

            foreach (var address in addresses)
            {
                addressBuilder.AppendLine(address);
            }

            return addressBuilder.ToString();
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Implementation of IModel

        public Result Create()
        {
            Action createRecord = () =>
            {
                string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME,
                                                                        SqlParameters);
                ID = DatabaseController.ExecuteInsertQuery(sql, SqlParameters.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                List<SqlParameter> sqlParameters = SqlParameters;
                string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
                                                                        sqlParameters, key);

                sqlParameters.Add(key);
                DatabaseController.ExecuteNonQuery(sql, sqlParameters.ToArray());
            };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                string sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

                DatabaseController.ExecuteNonQuery(sql, key);
            };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
            {
                ResetProperties();
                ID = id;

                var key = new SqlParameter("?ID", ID);
                string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

                DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SetPropertiesFromDataRow(dataRow);
                }
            };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            ID = 0;
            MemberCode = "";
            MemberName = "";
            IsMember = false;
            AreaCode = "";
            Address1 = "";
            Address2 = "";
            Address3 = "";
            HomeOwnership = "";
            Telephone = "";
            Birthday = new DateTime();
            Age = 0;
            BirthPlace = "";
            MembershipDate = new DateTime();
            Sex = "";
            CivilStatus = "";
            EducationalAttainment = "";
            Occupation = "";
            Business = "";
            OccupationIncome = 0m;
            BusinessIncome = 0m;
            SpouseOccupation = "";
            SpouseBusiness = "";
            SpouseOccupationIncome = 0m;
            SpouseBusinessIncome = 0m;
            IsAccountClosed = false;
            AccountClosedSince = new DateTime();
            Beneficiary1Name = "";
            Beneficiary1Age = 0;
            Beneficiary1Relationship = "";
            Beneficiary2Name = "";
            Beneficiary2Age = 0;
            Beneficiary2Relationship = "";
            Beneficiary3Name = "";
            Beneficiary3Age = 0;
            Beneficiary3Relationship = "";
            MembershipType = "";
            IsLetterSend = false;
            ProperName = "";
            ShareCapitalReceiptNo = "";
            ShareCapitalDepositDate = new DateTime();
            ShareCapitalAmount = 0m;
            IsDamayanMember = false;
            PreMembershipSeminarDate = new DateTime();
            DamayanMemberSince = new DateTime();
            CollectorName = "";
            BranchName = "";
            Department = "";
            Classification = "";
            StubNo = 0;
            PrecintNo = 0;
            
            ContactInformation = new Contact();
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            IsMember = DataConverter.ToBoolean(dataRow["MEMBER"]);
            AreaCode = DataConverter.ToString(dataRow["AREA_CODE"]);
            Address1 = DataConverter.ToString(dataRow["ADDRESS1"]);
            Address2 = DataConverter.ToString(dataRow["ADDRESS2"]);
            Address3 = DataConverter.ToString(dataRow["ADDRESS3"]);
            HomeOwnership = DataConverter.ToString(dataRow["HOUSE_STAT"]);
            Telephone = DataConverter.ToString(dataRow["TELEPHONE"]);
            Birthday = DataConverter.ToDateTime(dataRow["BIRTHDAY"]);
            //Age = DataConverter.ToInteger(dataRow["BAGE"]);
            BirthPlace = DataConverter.ToString(dataRow["BIRT_PLACE"]);
            MembershipDate = DataConverter.ToDateTime(dataRow["MEMBERSHIP"]);
            Sex = DataConverter.ToString(dataRow["SEX"]);
            CivilStatus = DataConverter.ToString(dataRow["CIVIL"]);
            EducationalAttainment = DataConverter.ToString(dataRow["DEGREE"]);
            Occupation = DataConverter.ToString(dataRow["OCCUPATION"]);
            Business = DataConverter.ToString(dataRow["BUSINESS"]);
            OccupationIncome = DataConverter.ToDecimal(dataRow["INCOME1"]);
            BusinessIncome = DataConverter.ToDecimal(dataRow["INCOME2"]);
            SpouseOccupation = DataConverter.ToString(dataRow["SPOUSE_OCC"]);
            SpouseBusiness = DataConverter.ToString(dataRow["SPOUSE_BUS"]);
            SpouseOccupationIncome = DataConverter.ToDecimal(dataRow["SPOUS_INC1"]);
            SpouseBusinessIncome = DataConverter.ToDecimal(dataRow["SPOUS_INC2"]);
            IsAccountClosed = DataConverter.ToBoolean(dataRow["CLOSE_ACCT"]);
            AccountClosedSince = DataConverter.ToDateTime(dataRow["CLOSE_DATE"]);
            Beneficiary1Name = DataConverter.ToString(dataRow["BENEFIT"]);
            Beneficiary1Age = DataConverter.ToInteger(dataRow["AGE"]);
            Beneficiary1Relationship = DataConverter.ToString(dataRow["RELATION"]);
            Beneficiary2Name = DataConverter.ToString(dataRow["BENEFIT1"]);
            Beneficiary2Age = DataConverter.ToInteger(dataRow["AGE1"]);
            Beneficiary2Relationship = DataConverter.ToString(dataRow["RELATION1"]);
            Beneficiary3Name = DataConverter.ToString(dataRow["BENEFIT2"]);
            Beneficiary3Age = DataConverter.ToInteger(dataRow["AGE2"]);
            Beneficiary3Relationship = DataConverter.ToString(dataRow["RELATION2"]);
            MembershipType = DataConverter.ToString(dataRow["MEM_TYPE"]);
            IsLetterSend = DataConverter.ToBoolean(dataRow["LETTER"]);
            ProperName = DataConverter.ToString(dataRow["NAME1"]);
            ShareCapitalReceiptNo = DataConverter.ToString(dataRow["OR_NO"]);
            ShareCapitalDepositDate = DataConverter.ToDateTime(dataRow["DATED"]);
            ShareCapitalAmount = DataConverter.ToDecimal(dataRow["SC_AMT"]);
            IsDamayanMember = DataConverter.ToBoolean(dataRow["DAMAYAN"]);
            PreMembershipSeminarDate = DataConverter.ToDateTime(dataRow["PMS"]);
            DamayanMemberSince = DataConverter.ToDateTime(dataRow["D_DATE"]);
            CollectorName = DataConverter.ToString(dataRow["COLLECTOR"]);
            BranchName = DataConverter.ToString(dataRow["BRANCH"]);
            Department = DataConverter.ToString(dataRow["DEPART"]);
            Classification = DataConverter.ToString(dataRow["CLASS"]);
            StubNo = DataConverter.ToInteger(dataRow["STUB_NO"]);
            PrecintNo = DataConverter.ToInteger(dataRow["PRECINT"]);

            ContactInformation = Contact.WhereMemberCodeIs(MemberCode);
        }

        public static Nfmb FindByCode(string code)
        {
            var member = new Nfmb();
            member.ResetProperties();
            var key = new SqlParameter("?MEM_CODE", code);
            string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                member.SetPropertiesFromDataRow(dataRow);
            }
            return member;
        }

        public static Nfmb FindByName(string name)
        {
            var member = new Nfmb();
            member.ResetProperties();
            var key = new SqlParameter("?MEM_NAME", name);
            string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                member.SetPropertiesFromDataRow(dataRow);
            }
            return member;
        }
        #endregion

        //public static Nfmb FindByMemberCode(string memberCode)
        //{
        //    var sqlBuilder = new StringBuilder();
        //    sqlBuilder.Append("SELECT * FROM nfmb WHERE MEM_CODE = ?MEM_CODE LIMIT 1");
        //    var param = new SqlParameter("?MEM_CODE", memberCode);

        //    var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), param);

        //    var member = new Nfmb();
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        member.SetPropertiesFromDataRow(dataRow);
        //    }
        //    return member;
        //}

        //public static Nfmb WhereMemberNameIs(string memberName)
        //{
        //    var sqlBuilder = new StringBuilder();
        //    sqlBuilder.Append("SELECT * FROM nfmb WHERE MEM_NAME = ?MEM_NAME LIMIT 1");
        //    var param = new SqlParameter("?MEM_NAME", memberName);

        //    var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), param);

        //    var member = new Nfmb();
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        member.SetPropertiesFromDataRow(dataRow);
        //    }
        //    return member;
        //}

        public static List<Nfmb> GetList()
        {
            const string sqlSelect = "SELECT ID, MEM_CODE, MEM_NAME FROM nfmb ORDER BY MEM_CODE";
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlSelect);
            var list = new List<Nfmb>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var member = new Nfmb();
                member.ID = DataConverter.ToInteger(dataRow["ID"]);
                member.MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
                member.MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
                list.Add(member);
            }
            return list;
        }
    }
}
