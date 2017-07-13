using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.ModelView
{
    public class UserManagementModelView : INotifyPropertyChanged
    {

        private Profile _profile = new Profile();

        public Profile profile
        {
            get { return _profile; }
            set
            {
                _profile = value;
                this.NotifyPropertyChanged("profile");
            }
        }

        private ObservableCollection<Profile> _profiles = new ObservableCollection<Profile>();

        public ObservableCollection<Profile> profiles
        {
            get { return _profiles; }
            set
            {
                _profiles = value;
                this.NotifyPropertyChanged("profiles");
            }
        }

        public double winHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight;
            }
        }

        public double winWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth;
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
