using BakeryPR.DAO;
using BakeryPR.Models;
using BakeryPR.Utilities;
using BakeryPR.Views;
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

        public DelegateCommand<object> loadUpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    EditProfile user = new EditProfile();
                    this.profile = (Profile)s;
                    user.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> updateUserCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(()=> {
                            
                    });
                });
            }
        }

        public DelegateCommand<object> loadAddCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {

                    AddUser user = new AddUser();
                    user.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> addUserCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    this.isSpin = Visibility.Visible;
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (!this.profile.pwd.Equals(this.profile.confirmPassword))
                            {
                                throw new Exception("Password does not match");
                            }
                            this.profile.status = UserSatus.ACTIVE.ToString();
                            byte[] bt = Encoding.ASCII.GetBytes(this.profile.pwd);
                            this.profile.pwd = Convert.ToBase64String(bt);
                            bool result = userDao.add(this.profile);
                            if (result)
                            {
                                this.profiles = new ObservableCollection<Profile>(userDao.all());
                                MessageBox.Show("User have been created", "Saved", MessageBoxButton.OK, 
                                    MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("User have not been created, Please try again or contact admin", "Error", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }

                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        this.isSpin = Visibility.Collapsed;
                    });
                });
            }
        }

        private Visibility _isSpin = Visibility.Collapsed;

        public Visibility isSpin
        {
            get { return _isSpin; }
            set
            {
                _isSpin = value;
                this.NotifyPropertyChanged("isSpin");
            }
        }


        private ObservableCollection<Profile> _profiles = new ObservableCollection<Profile>();

        public ObservableCollection<Profile> profiles
        {
            get
            {
                _profiles = new ObservableCollection<Profile>(userDao.all());
                return _profiles;
            }
            set
            {
                _profiles = value;
                this.NotifyPropertyChanged("profiles");
            }
        }

        public UserDao userDao
        {
            get
            {
                return new UserDao();
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
