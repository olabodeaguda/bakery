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
        public string title
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
        public ObservableCollection<DropdownModel> userStatus
        {
            get
            {
                List<DropdownModel> lst = new List<DropdownModel>()
                {
                    new DropdownModel(){ valuesId="-1", value="Select Status"}
                };

                lst.AddRange(userDao.lstUserStatus());
                return new ObservableCollection<DropdownModel>(lst);
            }
        }

        public DelegateCommand<object> ChangeUserStatusCommand
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
                            bool result = this.userDao.UpdateStatus(this.profile);
                            if (result)
                            {
                                MessageBox.Show("Saved", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            throw;
                        }
                    });
                    this.isSpin = Visibility.Collapsed;
                });
            }
        }

        public DelegateCommand<object> loadChangeUserStatus
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    ChangeUserStatus assignUR = new ChangeUserStatus();
                    this.profile = (Profile)s;
                    assignUR.DataContext = this;
                    assignUR.ShowDialog();
                });
            }
        }

        private UserRole _userRole = new UserRole();

        public UserRole userRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                this.NotifyPropertyChanged("userRole");
            }
        }

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
                List<Role> lstRole = new List<Role>() { new Role() { id = -1, name = "Select Role" } };
                lstRole.AddRange(this.roleDao.all());
                return new ObservableCollection<Role>(lstRole);
            }
        }
        private ObservableCollection<Role> _lstUserRole;

        public ObservableCollection<Role> lstUserRole
        {
            get
            {
                if (this.profile != null)
                {

                }
                return _lstUserRole;
            }
            set
            {
                _lstUserRole = value;
                this.NotifyPropertyChanged("lstUserRole");
            }
        }

        public DelegateCommand<object> addAssignUserRole
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (this.userRole.roleId == -1)
                            {
                                throw new Exception("Role is required!!");
                            }
                            bool result = roleDao.assignRoleTouser(this.userRole);
                            if (result)
                            {
                                MessageBox.Show("Saved", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                                this.lstUserRole = new ObservableCollection<Role>(roleDao.byProfileId(this.userRole.userId));
                                this.userRole = new UserRole();
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                });
            }
        }

        public DelegateCommand<object> deleteUserRoleCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            Role p = (Role)s;

                            MessageBoxResult msg = MessageBox.Show("Are you sure?", "Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (msg == MessageBoxResult.No)
                            {
                                return;
                            }

                            bool result = roleDao.deletebyProfileId(p.userId, p.id);
                            if (result)
                            {
                                MessageBox.Show("Deleted", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                                this.lstUserRole = new ObservableCollection<Role>(roleDao.byProfileId(this.userRole.userId));
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                });
            }
        }

        public DelegateCommand<object> loadAssignUserRole
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AssignUserRole assignUR = new AssignUserRole();
                    Profile p = (Profile)s;
                    this.userRole.userId = p.id;
                    this.lstUserRole = new ObservableCollection<Role>(roleDao.byProfileId(this.userRole.userId));
                    assignUR.DataContext = this;

                    assignUR.ShowDialog();
                });
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
                                throw new Exception("Username already exists");
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
                                throw new Exception(this.profile.username + " already exists");
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
