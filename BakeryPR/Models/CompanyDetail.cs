using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class CompanyDetail : INotifyPropertyChanged
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.NotifyPropertyChanged("id");
            }
        }

        private string _businessName;

        public string businessName
        {
            get { return _businessName; }
            set
            {
                _businessName = value;
                this.NotifyPropertyChanged("businessName");
            }
        }

        private string _businessAddress;

        public string businessAddress
        {
            get { return _businessAddress; }
            set
            {
                _businessAddress = value;
                this.NotifyPropertyChanged("businessAddress");
            }
        }

        private string _businessRegNUmber;

        public string businessRegNUmber
        {
            get { return _businessRegNUmber; }
            set
            {
                _businessRegNUmber = value;
                this.NotifyPropertyChanged("businessRegNUmber");
            }
        }

        private string _businessRegType;

        public string businessRegType
        {
            get { return _businessRegType; }
            set
            {
                _businessRegType = value;
                this.NotifyPropertyChanged("businessRegType");
            }
        }

        private string _emailAddress;

        public string emailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                this.NotifyPropertyChanged("emailAddress");
            }
        }

        private string _contactName;

        public string contactName
        {
            get { return _contactName; }
            set
            {
                _contactName = value;
                this.NotifyPropertyChanged("contactName");
            }
        }

        private string _contactPhoneNumber;

        public string contactPhoneNumber
        {
            get { return _contactPhoneNumber; }
            set
            {
                _contactPhoneNumber = value;
                this.NotifyPropertyChanged("contactPhoneNumber");
            }
        }

        private string _contactEmail;

        public string contactEmail
        {
            get { return _contactEmail; }
            set
            {
                _contactEmail = value;
                this.NotifyPropertyChanged("contactEmail");
            }
        }

        #region property change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
