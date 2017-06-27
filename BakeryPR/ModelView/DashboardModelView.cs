using BakeryPR.Utilities;
using BakeryPR.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BakeryPR.ModelView
{
    public class DashboardModelView: INotifyPropertyChanged
    {
        public void Navigate(object obj)
        {
            string ss = (string)obj;
            if (ss == null)
            {
                ss = PageParameter.dashboard.ToString();
            }
            this.navState = NavigateUtils.getNav(ss);
            this.NotifyPropertyChanged("navState");
        }

        public DelegateCommand<object> navCommand
        {
            get
            {
                return new DelegateCommand<object>(Navigate);
            }
        }

        private object _navState;

        public object navState
        {
            get { return _navState; }
            set { _navState = value;
                this.NotifyPropertyChanged("navState");
            }
        }


        public double winHeight {
            get {
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

        public string footerText
        {
            get
            {
                return "Copyright © " + DateTime.Now.Year.ToString();
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
