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

namespace BakeryPR.ModelView
{
    public class LoginModelView : INotifyPropertyChanged, ICloseable
    {
        private LoginModel _loginmodel = new LoginModel();

        public LoginModel loginModel
        {
            get
            {
                return _loginmodel;
            }
            set
            {
                _loginmodel = value;
                this.NotifyPropertyChanged("loginModel");
            }
        }

        public AppConfigDao appconfigDao
        {
            get
            {
                return new AppConfigDao();
            }
        }

        
        public DelegateCommand<object> loginCommand
        {
            get
            {
                return new DelegateCommand<object>(async (S) =>
                {
                    this.isSpin = Visibility.Visible;
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(this.loginModel.username))
                            {
                                throw new Exception("Username is required");
                            }
                            else if (string.IsNullOrEmpty(this.loginModel.pwd))
                            {
                                throw new Exception("User password is required");
                            }
                            else if (string.IsNullOrEmpty(this.loginModel.pwd.Trim()))
                            {
                                throw new Exception("User password is required");
                            }

                            string pass = Convert.ToBase64String(Encoding.ASCII.GetBytes(this.loginModel.pwd.Trim()));

                            Profile details = userDao.validateCredentials(new LoginModel()
                            {
                                username = this.loginModel.username,
                                pwd = pass
                            });

                            if (details == null)
                            {
                                throw new Exception("User/Password is incorrect");
                            }

                            if (details.status == UserSatus.ACTIVE.ToString())
                            {
                                appconfigDao.updateConfig(new LoginModel()
                                {
                                    fullname = details.fullName,
                                    status = details.status,
                                    username = details.username,
                                    isLogin = "1"
                                }); 
                            }
                            else
                            {
                                throw new Exception("user status is " + details.status);
                            }

                            this.loginModel = appconfigDao.read();

                            this.isSpin = Visibility.Collapsed;
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    });

                    if (this.loginModel.isLogin != null)
                    {
                        if (this.loginModel.isLogin.Equals("1"))
                        {
                            LoginView lv = (LoginView)Application.Current.MainWindow;
                            Dashboard dh = new Dashboard();
                            dh.Show();
                            lv.Close();
                        } 
                    }
                });
            }
        }

        private UserDao userDao
        {
            get
            {
                return new UserDao();
            }
        }

        private Visibility _isSpin = Visibility.Collapsed;

        public Visibility isSpin
        {
            get { return _isSpin; }
            set { _isSpin = value; }
        }


        public void close()
        {
            if (this is ICloseable)
            {
                (this as ICloseable).RequestClose += (_, __) => this.close();
            }
        }

        #region property change

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<EventArgs> RequestClose;

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
