using BakeryPR.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class UserStatusModel : INotifyPropertyChanged
    {
        private AppConfigDao _appConfigDao;

        public AppConfigDao appConfigDao
        {
            get { return _appConfigDao; }
            set
            {
                _appConfigDao = value;
                this.NotifyPropertyChanged("appConfigDao");
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

        private string _status;

        public string status
        {
            get { return _status; }
            set
            {
                _status = value;
                this.NotifyPropertyChanged("sttaus");
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
