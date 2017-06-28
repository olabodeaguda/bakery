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
    public class ProductViewModel : INotifyPropertyChanged
    {

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s)=>{
                    AddProduct prod = new AddProduct();
                    prod.ShowDialog();
                    this.products = new ObservableCollection<Product>(dao.all());
                });
            }
        }

        public ProductDao dao
        {
            get
            {
                return new ProductDao();
            }
        }

        private MeasureTypeDao mDao
        {
            get
            {
                return new MeasureTypeDao();
            }
        }

        private Product _product;

        public Product product
        {       
            get { return _product; }
            set {
                _product = value;
                this.NotifyPropertyChanged("product");
            }
        }


        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> products
        {
            get
            {
                _products = new ObservableCollection<Product>(dao.all());
                return _products;
            }
            set
            {
                _products = value;
                this.NotifyPropertyChanged("products");
            }
        }

        public ObservableCollection<MeasurementType> MeasurementTypes
        {
            get
            {
                List<MeasurementType> lst = new List<MeasurementType>();
                lst.Add(new MeasurementType() { id = -1, name = "none" });
                lst.AddRange(mDao.all());
                return new ObservableCollection<MeasurementType>(lst);
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
