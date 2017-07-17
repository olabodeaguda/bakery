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

        public RoleDao roleDao
        {
            get
            {
                return new RoleDao();
            }
        }

        public ObservableCollection<Role> roles
        {
            get
            {
                return new ObservableCollection<Role>(this.roleDao.all());
            }
        }


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
                    user.DataContext = this;
                    user.ShowDialog();
                    this.profile = new Profile();
                });
            }
        }

        private ChangePass _changePass = new ChangePass();

        public ChangePass changePass
        {
            get { return _changePass; }
            set
            {
                _changePass = value;
                this.NotifyPropertyChanged("changePass");
            }
        }

        public DelegateCommand<object> updateUserCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            Profile res = userDao.byProfile(this.profile);
                            if (res != null)
                            {
                                throw new Exception("Username already exist for another user");
                            }
                            bool result = userDao.Update(this.profile);
                            if (result)
                            {
                                MessageBox.Show("Saved");
                                this.profiles = new ObservableCollection<Profile>(userDao.all());
                            }
                            else
                            {
                                MessageBox.Show("not saved");
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message);
                        }

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
                    user.DataContext = this;
                    user.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> loadChangePwdCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    Profile p = (Profile)s;
                    ChangePwd user = new ChangePwd();
                    this.changePass.id = p.id;
                    user.DataContext = this;
                    user.ShowDialog();
                    this.changePass = new ChangePass();
                });
            }
        }

        public DelegateCommand<object> ChangePwdCommand
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
                            if (!this.changePass.newPassword.Equals(this.changePass.confirmPassword))
                            {
                                throw new Exception("Password does not match");
                            }

                            byte[] bt = Encoding.ASCII.GetBytes(this.changePass.newPassword);
                            this.changePass.newPassword = Convert.ToBase64String(bt);
                            bool result = userDao.ChangePwd(this.changePass);
                            if (result)
                            {
                                MessageBox.Show("User Password have been changed successfully", "Change Password", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                this.profile = new Profile();
                            }
                            else
                            {
                                MessageBox.Show("User Password have not been changed successfully, Please try again or contact admin", "Error", MessageBoxButton.OK,
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

                            //check if user already exist
                            Profile pFile = userDao.byUsername(this.profile.username);
                            if (pFile != null)
                            {
                                throw new Exception(this.profile.username + " already exist");
                            }

                            this.profile.status = UserSatus.ACTIVE.ToString();
                            byte[] bt = Encoding.ASCII.GetBytes(this.profile.pwd);
                            this.profile.pwd = Convert.ToBase64String(bt);
                            bool result = userDao.add(this.profile);
                            if (result)
                            {
                                MessageBox.Show("User have been created", "Saved", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                                this.profile = new Profile();
                                this.profiles = new ObservableCollection<Profile>(userDao.all());
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


        private ObservableCollection<Profile> _profiles;

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
