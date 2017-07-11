using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class UserRole : INotifyPropertyChanged
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

        private int _userid;

        public int userId
        {
            get { return _userid; }
            set
            {
                _userid = value;
                this.NotifyPropertyChanged("userid");
            }
        }

        private int _roleId;

        public int roleId
        {
            get { return _roleId; }
            set
            {
                _roleId = value;
                this.NotifyPropertyChanged("roleId");
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
