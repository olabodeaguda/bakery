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

        public DelegateCommand<object> reportCommand
        {
            get
            {
                return new DelegateCommand<object>(async(s) =>
                {
                    await Task.Run(() =>
                    {
                        // generate report
                    });
                });
            }
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddProduct prod = new AddProduct();
                    prod.DataContext = this;
                    prod.ShowDialog();
                    this.products = new ObservableCollection<Product>(dao.all());
                });
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
                        Product sd = this.product;
                        bool result = dao.update(sd);
                        if (result)
                        {
                            MessageBox.Show("Update was successfull");
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                });
            }
        }

        public DelegateCommand<object> addCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        Product ig = this.product;

                        bool result = dao.add(ig);
                        if (result)
                        {
                            MessageBox.Show("Saved");
                        }
                        else
                        {
                            MessageBox.Show("not saved");
                        }
                        this.product = new Product();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
            }
        }

        public DelegateCommand<object> loadEditCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    if (s != null)
                    {
                        EditProduct editIngr = new EditProduct();
                        this.product = (Product)s;

                        editIngr.DataContext = this;
                        bool? result = editIngr.ShowDialog();
                        this.product = new Product();
                        this.products = new ObservableCollection<Product>(dao.all());
                    }
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

        private Product _product = new Product();

        public Product product
        {
            get { return _product; }
            set
            {
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
