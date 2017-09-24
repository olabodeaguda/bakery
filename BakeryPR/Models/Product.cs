using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class Product : INotifyPropertyChanged
    {
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

        private double _weight;

        public double weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                this.NotifyPropertyChanged("weight");
            }
        }

        private string _descripton;

        public string descripton
        {
            get { return _descripton; }
            set
            {
                _descripton = value;
                this.NotifyPropertyChanged("description");
            }
        }

        private double _costOfPackage;

        public double costOfPackage
        {
            get { return _costOfPackage; }
            set
            {
                _costOfPackage = value;
                this.NotifyPropertyChanged("costOfPackage");
            }
        }

        private double _retailPrice;

        public double retailPrice
        {
            get { return _retailPrice; }
            set
            {
                _retailPrice = value;
                this.NotifyPropertyChanged("retailPrice");
            }
        }

        private double _wholeSales;

        public double wholeSales
        {
            get { return _wholeSales; }
            set
            {
                _wholeSales = value;
                this.NotifyPropertyChanged("wholeSales");
            }
        }

        private int _mTypeId = -1;

        public int mTypeId
        {
            get { return _mTypeId; }
            set
            {
                _mTypeId = value;
                this.NotifyPropertyChanged("mTypeId");
            }
        }

        private string _measureTypeName;

        public string measureTypeName
        {
            get { return _measureTypeName; }
            set
            {
                _measureTypeName = value;
                this.NotifyPropertyChanged("measureTypeName");
            }
        }

        public string weightView
        {
            get
            {
                return $"{ this.weight} {this.measureTypeName}";
            }
        }

        private string _name;

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.NotifyPropertyChanged("name");
            }
        }

        private int _inventoryStore;

        public int inventoryStore
        {
            get { return _inventoryStore; }
            set
            {
                _inventoryStore = value;
                this.NotifyPropertyChanged("inventoryStore");
            }
        }

        private bool _isDiscount;

        public bool isDiscount
        {
            get { return _isDiscount; }
            set
            {
                _isDiscount = value;
                this.NotifyPropertyChanged("isDiscount");
            }
        }

        private double _discount;

        public double discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                this.NotifyPropertyChanged("discount");
            }
        }

        private string _quantityString;

        public string quantityString
        {
            get
            {
                return $"Quantity: {inventoryStore}";
            }
            set
            {
                quantityString = value;
                this.NotifyPropertyChanged("quantityString");
            }
        }


        public string retailString
        {
            get
            {
                return $"Retail: {retailPrice}";
            }
        }


        public string wholeString
        {
            get
            {
                return $"Wholesales: {wholeSales}";
            }
        }

        private double _costprice;

        public double costprice
        {
            get { return _costprice; }
            set
            {
                _costprice = value;
                this.NotifyPropertyChanged("costprice");
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
