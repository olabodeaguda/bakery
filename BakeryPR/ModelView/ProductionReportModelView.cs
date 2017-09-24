using BakeryPR.DAO;
using BakeryPR.Models;
using BakeryPR.Utilities;
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
    public class ProductionReportModelView : INotifyPropertyChanged
    {
        public ProductionReportDao productionReportDao
        {
            get
            {
                return new ProductionReportDao();
            }
        }

        private ReportSearchModel _reportSearchModel = new ReportSearchModel();

        public ReportSearchModel reportSearchModel
        {
            get { return _reportSearchModel; }
            set
            {
                _reportSearchModel = value;
                this.NotifyPropertyChanged("reportSearchModel");
            }
        }

        private Visibility _isLoading = Visibility.Collapsed;

        public Visibility isLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                this.NotifyPropertyChanged("isLoading");
            }
        }

        public DelegateCommand<object> SearchCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    if (string.IsNullOrEmpty(this.reportSearchModel.startDateDisplay))
                    {
                        return;
                    }
                    this.isLoading = Visibility.Visible;
                    await Task.Run(() =>
                    {
                        if (!string.IsNullOrEmpty(this.reportSearchModel.startDateDisplay) &&
                            !string.IsNullOrEmpty(this.reportSearchModel.endDateDisplay))
                        {
                            long startDate = new DateTime(this.reportSearchModel.startDate.Year,
                                this.reportSearchModel.startDate.Month, this.reportSearchModel.startDate.Day).Ticks;
                            long endDate = new DateTime(this.reportSearchModel.endDate.Year,
                                this.reportSearchModel.endDate.Month, this.reportSearchModel.endDate.Day).Ticks;
                            searchResult = new ObservableCollection<ProductionReportModel>(productionReportDao.ByDate(startDate, endDate));
                        }
                        else if (!string.IsNullOrEmpty(this.reportSearchModel.startDateDisplay))
                        {
                            long startDate = new DateTime(this.reportSearchModel.startDate.Year,
                                this.reportSearchModel.startDate.Month, this.reportSearchModel.startDate.Day).Ticks;
                            searchResult = new ObservableCollection<ProductionReportModel>(productionReportDao.ByDate(startDate));
                        }

                    });
                    this.isLoading = Visibility.Collapsed;
                });
            }
        }



        private ObservableCollection<ProductionReportModel> _searchResult = new ObservableCollection<ProductionReportModel>();

        public ObservableCollection<ProductionReportModel> searchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value;
                this.NotifyPropertyChanged("searchResult");
            }
        }

        #region property change

        public double winHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight - 100;
            }
        }

        public double winWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth;
            }
        }

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
