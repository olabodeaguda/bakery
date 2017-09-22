using BakeryPR.DAO;
using BakeryPR.ls;
using BakeryPR.Models;
using BakeryPR.Utilities;
using BakeryPR.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.ModelView
{
    public class ValidateLicenseModelView : INotifyPropertyChanged
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
        private CompanyDetail _companyDetail = new CompanyDetail();

        public CompanyDetail companyDetail
        {
            get { return _companyDetail; }
            set { _companyDetail = value; }
        }

        private LicenseModel _licenseModel = new LicenseModel();

        public LicenseModel licenseModel
        {
            get { return _licenseModel; }
            set
            {
                _licenseModel = value;
                this.NotifyPropertyChanged("licenseModel");
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


        public LicenseDao licenseDao
        {
            get
            {
                return new LicenseDao();
            }
        }

        public DelegateCommand<object> AddCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    try
                    {

                        if (string.IsNullOrEmpty(licenseModel.value))
                        {
                            throw new Exception("Key is required!!!");
                        }

                        isSpin = Visibility.Visible;

                        LicenseModel r = await Keys.getRemoteKeys(licenseModel.value);

                        bool result = false;
                        LicenseModel d = licenseDao.get();
                        if (d == null)
                        {
                            var f = companyDetailDao.All();
                            if (f != null)
                            {
                                r.appName = f.businessName;
                            }
                            result = licenseDao.Add(r);
                        }
                        else
                        {
                            r.id = d.id;
                            result = licenseDao.Update(r);
                        }

                        isSpin = Visibility.Collapsed;
                        if (result)
                        {
                            AppConfigUtils.Write("key", licenseModel.value);
                            MessageBox.Show("Key validation successfull");
                            ValidateLicense vl = null;
                            foreach (Window tm in Application.Current.Windows)
                            {
                                if (tm is ValidateLicense)
                                {
                                    vl = (ValidateLicense)tm;
                                    break;
                                }
                            }

                            LoginView loginView = new LoginView();
                            loginView.Show();
                            if (vl != null)
                            {
                                vl.Close();
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        isSpin = Visibility.Collapsed;
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
