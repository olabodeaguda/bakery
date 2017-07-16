using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class LoginModel : INotifyPropertyChanged
    {
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

        private string _fullname;

        public string fullname
        {
            get { return _fullname; }
            set
            {
                _fullname = value;
                this.NotifyPropertyChanged("fullname");
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
        private string _isLogin;

        public string isLogin
        {
            get { return _isLogin; }
            set
            {
                _isLogin = value;
                this.NotifyPropertyChanged("isLogin");
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
