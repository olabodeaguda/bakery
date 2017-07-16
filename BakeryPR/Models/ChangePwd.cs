using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.Models
{
    public class ChangePass : INotifyPropertyChanged
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

        private string _oldPassword;

        public string oldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                this.NotifyPropertyChanged("oldPassword");
            }
        }

        private string _newPassword;

        public string newPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                this.NotifyPropertyChanged("newPassword");
            }
        }

        private string _confirmPassword;

        public string confirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                this.NotifyPropertyChanged("confirmPassword");
            }
        }


        #region property change

        public double winHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight - 100;
            }
        }

        public double winWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth;
            }
        }

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
