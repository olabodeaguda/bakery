using BakeryPR.DAO;
using BakeryPR.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class CartProductModel : INotifyPropertyChanged
    {
        public ProductDao pDao
        {
            get
            {
                return new ProductDao();
            }
        }

        private bool _isRetail = false;

        public bool isRetail
        {
            get { return _isRetail; }
            set
            {
                _isRetail = value;
                this.NotifyPropertyChanged("isRetail");
            }
        }

        public DelegateCommand<object> quantityChangedCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                   // Product p = pDao.byId(this.productId);
                    //if (this.isRetail)
                    //{
                    //    //this.price = this.quantity * p.retailPrice;
                    //    this.price = this.quantity * this.manualPrice;
                    //}
                    //else
                    //{
                    //    this.price = this.quantity * p.wholeSales;
                    //}
                    this.price = this.quantity * this.manualPrice;
                });
            }
        }

        private int _id;

        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.NotifyPropertyChanged("id");
            }
        }

        private double _price;

        public double price
        {
            get { return _price; }
            set
            {
                _price = value;
                this.NotifyPropertyChanged("price");
            }
        }

        private double _manualPrice;

        public double manualPrice
        {
            get { return _manualPrice; }
            set
            {
                _manualPrice = value;
                this.NotifyPropertyChanged("manualPrice");
            }
        }

        private int _quantity;

        public int quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                int pId = this.productId;
                this.NotifyPropertyChanged("quantity");
            }
        }

        private int _productId;

        public int productId
        {
            get { return _productId; }
            set
            {
                _productId = value;
                this.NotifyPropertyChanged("productId");
            }
        }

        private int _cartId;

        public int cartId
        {
            get { return _cartId; }
            set
            {
                _cartId = value;
                this.NotifyPropertyChanged("cartId");
            }
        }

        private string _productName;

        public string productName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                this.NotifyPropertyChanged("productName");
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
