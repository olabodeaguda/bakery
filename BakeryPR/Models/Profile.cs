using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class Profile : INotifyPropertyChanged
    {
        private string _id;

        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.NotifyPropertyChanged("id");
            }
        }

        private string _username;

        public string username
        {
            get { return _username; }
            set
            {
                _username = value;
                this.NotifyPropertyChanged("username");
            }
        }

        private string _surname;

        public string surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                this.NotifyPropertyChanged("surname");
            }
        }

        private string _othername;

        public string othername
        {
            get { return _othername; }
            set
            {
                _othername = value;
                this.NotifyPropertyChanged("othername");
            }
        }

        private string _status;

        public string status
        {
            get { return _status; }
            set
            {
                _status = value;
                this.NotifyPropertyChanged("status");
            }
        }

        private string _pwd;

        public string pwd
        {
            get { return _pwd; }
            set
            {
                _pwd = value;
                this.NotifyPropertyChanged("pwd");
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
