using BakeryPR.DAO;
using BakeryPR.Models;
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
    public class DashboardModelView : INotifyPropertyChanged
    {
        public string titleBar
        {
            get
            {
                return companyDetailDao.Title();
            }
        }

        public CompanyDetailDao companyDetailDao
        {
            get
            {
                return new CompanyDetailDao();
            }
        }


        public void Navigate(object obj)
        {
            this.userModel = appDao.read();
            string ss = (string)obj;
            if (ss == null)
            {
                ss = PageParameter.dashboard.ToString();
            }
            string tt = "";
            this.navState = NavigateUtils.getNav(ss, out tt);
            this.title = tt;
            this.isSpin = false;
            this.NotifyPropertyChanged("navState");
        }

        public DelegateCommand<object> logoutCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    Dashboard dh = null;
                    foreach (Window tm in Application.Current.Windows)
                    {
                        if (tm is Dashboard)
                        {
                            dh = (Dashboard)tm;
                            break;
                        }
                    }
                    if (dh != null)
                    {
                        appConfigDao.updateConfig(new LoginModel()
                        {
                            username = "",
                            isLogin = "0",
                            status = ""
                        });
                        LoginView lv = new LoginView();
                        lv.Show();
                        dh.Close();
                    }
                });
            }
        }

        private LoginModel _loginModel;

        public LoginModel loginModel
        {
            get
            {
                _loginModel = this.appConfigDao.read();
                return _loginModel;
            }
            set
            {
                _loginModel = value;
                this.NotifyPropertyChanged("loginModel");
            }
        }

        private Visibility _isLogin = Visibility.Collapsed;

        public Visibility isLogin
        {
            get
            {
                if (this.loginModel.isLogin.Equals("1"))
                {
                    _isLogin = Visibility.Visible;
                }
                else
                {
                    _isLogin = Visibility.Collapsed;
                }
                return _isLogin;
            }
            set
            {
                _isLogin = value;
                this.NotifyPropertyChanged("isLogin");
            }
        }

        private Visibility _isAccessible = Visibility.Collapsed;

        public Visibility isAccessible
        {
            get
            {
                if (this.loginModel.role.ToLower().Equals("administrator"))
                {
                    _isAccessible = Visibility.Visible;
                }
                else
                {
                    _isAccessible = Visibility.Collapsed;
                }
                return _isAccessible;
            }
            set
            {
                _isAccessible = value;
                this.NotifyPropertyChanged("");
            }
        }


        public AppConfigDao appConfigDao
        {
            get
            {
                return new AppConfigDao();
            }
        }

        private string _title = "";

        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.NotifyPropertyChanged("title");
            }
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
            set
            {
                _navState = value;
                this.NotifyPropertyChanged("navState");
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

        public double winWidthHalf
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth / 2;
            }
        }

        public string footerText
        {
            get
            {
                return " FMN Bakery Manager 1.0 © Flourmills of Nigeria PLC " +  DateTime.Now.Year.ToString();
            }
        }

        private bool _isSpin = true;

        public bool isSpin
        {
            get { return _isSpin; }
            set
            {
                _isSpin = value;
                this.NotifyPropertyChanged("isSpin");
            }
        }

        public AppConfigDao appDao
        {
            get
            {
                return new AppConfigDao();
            }
        }

        private LoginModel _userModel = new LoginModel();

        public LoginModel userModel
        {
            get
            {
                _userModel = appDao.read();
                return _userModel;
            }
            set
            {
                _userModel = value;

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
