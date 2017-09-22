using BakeryPR.DAO;
using BakeryPR.Models;
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


        public List<Product> products
        {
            get
            {
                return this.productDao.all();
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
