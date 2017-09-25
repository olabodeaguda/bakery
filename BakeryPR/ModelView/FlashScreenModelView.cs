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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BakeryPR.ModelView
{
    public class FlashScreenModelView : INotifyPropertyChanged
    {
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

        public LicenseDao licenseDao
        {
            get
            {
                return new LicenseDao();
            }
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Start();

                });
            }
        }
        int tickCount = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            tickCount += 1;
            if (tickCount == 6)
            {
                loadFinished();
            }
        }

        private void loadFinished()
        {
            var r = companyDetailDao.All();
            if (r == null)
            {
                CompanyDetails company = new CompanyDetails();
                CompanyDetailModelView cmv = new CompanyDetailModelView();
                company.DataContext = cmv;
                FlashScreen lv = (FlashScreen)Application.Current.MainWindow;
                company.Show();
                if (lv != null)
                {
                    lv.Close();
                }
            }
            else
            {
                //validate license 
                var e = licenseDao.get();
                if (e == null)
                {
                    ValidateLicense vl = new ValidateLicense();
                    ValidateLicenseModelView vmv = new ValidateLicenseModelView();
                    vmv.companyDetail = r;
                    vl.DataContext = vmv;
                    FlashScreen lv = (FlashScreen)Application.Current.MainWindow;
                    vl.Show();
                    if (lv != null)
                    {
                        lv.Close();
                    }

                }
                else
                {
                    int count = 0;
                    if (e.loadCount != 0 && (e.loadCount % 4000) == 0)
                    {
                        count++;
                    }
                    else if (!Keys.ValidateKey(e,r.businessName))
                    {
                        count++;
                    }

                    FlashScreen currentWindow = null;
                    foreach (Window tm in Application.Current.Windows)
                    {
                        if (tm is FlashScreen)
                        {
                            currentWindow = (FlashScreen)tm;
                            break;
                        }
                    }

                    if (count > 0)
                    {
                        ValidateLicense flsh = new ValidateLicense();
                        ValidateLicenseModelView vmv = new ValidateLicenseModelView();
                        vmv.companyDetail = r;
                        flsh.DataContext = vmv;
                        flsh.Show();
                    }
                    else
                    {
                        LoginView lg = new LoginView();
                        lg.Show();
                    }

                    if (currentWindow != null)
                    {
                        currentWindow.Close();
                    }
                }
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
