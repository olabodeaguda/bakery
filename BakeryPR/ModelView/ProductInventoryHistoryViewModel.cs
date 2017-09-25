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
    public class ProductInventoryHistoryViewModel : INotifyPropertyChanged
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
        public ProductInventoryHistoryDao piDao
        {
            get
            {
                return new ProductInventoryHistoryDao();
            }
        }

        public ProductDao productDao
        {
            get
            {
                return new ProductDao();
            }
        }

        private int _selectedproduct;

        public int selectedproduct
        {
            get { return _selectedproduct; }
            set
            {
                _selectedproduct = value;
                this.productHistory = new ObservableCollection<ProductInventoryHistory>(piDao.byId(value));
                this.NotifyPropertyChanged("selectedproduct");
            }
        }

        private ProductInventoryHistoryModel _productInventoryHistoryModel = new ProductInventoryHistoryModel();

        public ProductInventoryHistoryModel productInventoryHistoryModel
        {
            get { return _productInventoryHistoryModel; }
            set
            {
                _productInventoryHistoryModel = value;
                this.NotifyPropertyChanged("productInventoryHistoryModel");
            }
        }

        private Visibility _isBusyVisible = Visibility.Collapsed;

        public Visibility isBusyVisible
        {
            get { return _isBusyVisible; }
            set
            {
                _isBusyVisible = value;
                this.NotifyPropertyChanged("isBusyVisible");
            }
        }


        public DelegateCommand<object> searchHistoryCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    isBusyVisible = Visibility.Visible;
                    await Task.Run(() =>
                    {
                        string query = string.Empty;
                        if (string.IsNullOrEmpty(productInventoryHistoryModel.startDateDisplay) && string.IsNullOrEmpty(productInventoryHistoryModel.endDateDisplay))
                        {
                            query = piDao.SearchQuery(this.productInventoryHistoryModel.selectedproduct);
                        }
                        else if (!string.IsNullOrEmpty(productInventoryHistoryModel.startDateDisplay) && string.IsNullOrEmpty(productInventoryHistoryModel.endDateDisplay))
                        {
                            query = piDao.SearchQuery(this.productInventoryHistoryModel.selectedproduct, this.productInventoryHistoryModel.startDate.Ticks);
                        }
                        else if (!string.IsNullOrEmpty(productInventoryHistoryModel.startDateDisplay) && !string.IsNullOrEmpty(productInventoryHistoryModel.endDateDisplay))
                        {
                            query = piDao.SearchQuery(this.productInventoryHistoryModel.selectedproduct, this.productInventoryHistoryModel.startDate.Ticks,
                                this.productInventoryHistoryModel.endDate.Ticks);
                        }

                        this.productHistory = new ObservableCollection<ProductInventoryHistory>(piDao.byId(query));
                    });
                    isBusyVisible = Visibility.Collapsed;
                });
            }
        }


        public List<Product> products
        {
            get
            {
                List<Product> lst = new List<Product>();
                lst.Add(new Product()
                {
                    id = -1,
                    name = "Select product"
                });
                lst.AddRange(this.productDao.all());
                return lst;
            }
        }

        private ObservableCollection<ProductInventoryHistory> _productHistory = new ObservableCollection<ProductInventoryHistory>();

        public ObservableCollection<ProductInventoryHistory> productHistory
        {
            get { return _productHistory; }
            set
            {
                _productHistory = value;
                this.NotifyPropertyChanged("productHistory");
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
