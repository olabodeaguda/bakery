using BakeryPR.DAO;
using BakeryPR.ls;
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
    public class CompanyDetailModelView : INotifyPropertyChanged
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
        private Visibility _canAdd = Visibility.Collapsed;

        public Visibility canAdd
        {
            get { return _canAdd; }
            set
            {
                _canAdd = value;
                this.NotifyPropertyChanged("canAdd");
            }
        }

        private bool _isAllowEdit = false;

        public bool isAllowEdit
        {
            get { return _isAllowEdit; }
            set
            {
                _isAllowEdit = value;
                this.NotifyPropertyChanged("isAllowEdit");
            }
        }


        public DelegateCommand<object> allowEdit
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    isAllowEdit = !isAllowEdit;
                });
            }
        }

        public DelegateCommand<object> InitializationCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        var r = companyDetailDao.All();
                        this.companyDetail = r;
                        if (r == null)
                        {
                            canAdd = Visibility.Visible;
                        }
                    });
                });
            }
        }

        private CompanyDetail _companyDetail = new CompanyDetail();

        public CompanyDetail companyDetail
        {
            get { return _companyDetail; }
            set
            {
                _companyDetail = value;
                this.NotifyPropertyChanged("companyDetail");
            }
        }

        public LicenseDao licenseDao
        {
            get
            {
                return new LicenseDao();
            }
        }

        public DelegateCommand<object> UpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        MessageBoxResult br = MessageBox.Show("Are you sure?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (br == MessageBoxResult.No)
                        {
                            return;
                        }
                        else
                        {

                            if (!ValidateField.IsValidEmailAddress(this.companyDetail.contactEmail))
                            {
                                throw new Exception("Contact Email is invalid");
                            }
                            else if (!ValidateField.IsValidEmailAddress(this.companyDetail.emailAddress))
                            {
                                throw new Exception("Business Email is invalid");
                            }

                            var company = companyDetailDao.All();
                            bool isLicensed = Keys.ValidateKey(licenseDao.get(),company.businessName);

                            if (isLicensed)
                            {
                                LicenseModel licenseModel = licenseDao.getFull();
                                licenseModel.appName = companyDetail.businessName;
                                bool license = licenseDao.UpdateComplete(licenseModel);

                                if (license)
                                {
                                    bool isAdded = companyDetailDao.Update(this.companyDetail);

                                    if (isAdded)
                                    {
                                        MessageBox.Show("Saved", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.isAllowEdit = false;
                                    } 
                                }
                                else
                                {
                                    throw new Exception("An error occur while trying to update pre-company details");
                                }
                            }
                            else
                            {
                                throw new Exception("License is invalid");
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        this.companyDetail = companyDetailDao.All();
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                });
            }
        }

        public DelegateCommand<object> AddCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        if (!ValidateField.IsValidEmailAddress(this.companyDetail.contactEmail))
                        {
                            throw new Exception("Contact Email is invalid");
                        }
                        else if (!ValidateField.IsValidEmailAddress(this.companyDetail.emailAddress))
                        {
                            throw new Exception("Business Email is invalid");
                        }

                        bool isAdded = companyDetailDao.Add(this.companyDetail);

                        if (isAdded)
                        {
                            //validate Company
                            ValidateLicense vl = new ValidateLicense();
                            ValidateLicenseModelView vlmv = new ValidateLicenseModelView();
                            vlmv.companyDetail = this.companyDetail;
                            vl.DataContext = vlmv;
                            CompanyDetails cpD = null;// (CompanyDetails)Application.Current.MainWindow;
                            foreach (Window tm in Application.Current.Windows)
                            {
                                if (tm is CompanyDetails)
                                {
                                    cpD = (CompanyDetails)tm;
                                    break;
                                }
                            }

                            vl.Show();
                            if (cpD != null)
                            {
                                cpD.Close();
                            }

                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                });
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
