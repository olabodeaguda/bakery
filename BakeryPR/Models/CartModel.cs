using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class CartModel : INotifyPropertyChanged
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

        public double totalSales
        {
            get
            {
                return quantity * price;
            }
        }

        private double _retailPrice;

        public double retailPrice
        {
            get { return _retailPrice; }
            set { _retailPrice = value; }
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


        private string _customerName;

        public string customerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                this.NotifyPropertyChanged("customerName");
            }
        }

        private string _salesType;

        public string salesType
        {
            get { return _salesType; }
            set
            {
                _salesType = value;
                this.NotifyPropertyChanged("salesType");
            }
        }

        private bool _isWholesales = false;

        public bool isWholesales
        {
            get { return _isWholesales; }
            set
            {
                _isWholesales = value;
                this.NotifyPropertyChanged("isWholesales");
            }

        }

        private bool _isRetails = true;

        public bool isRetails
        {
            get { return _isRetails; }
            set
            {
                _isRetails = value;
                this.NotifyPropertyChanged("isRetails");
            }
        }



        private DateTime _dateCreated;

        public DateTime dateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                this.NotifyPropertyChanged("dateCreated");
            }
        }

        private string _createdBy;

        public string createdBy
        {
            get { return _createdBy; }
            set
            {
                _createdBy = value;
                this.NotifyPropertyChanged("createdBy");
            }
        }

        private string _cartStatus;

        public string cartStatus
        {
            get { return _cartStatus; }
            set
            {
                _cartStatus = value;
                this.NotifyPropertyChanged("cartStatus");
            }
        }

        private string _lastModifiedBy;

        public string lastModifiedBy
        {
            get { return _lastModifiedBy; }
            set
            {
                _lastModifiedBy = value;
                this.NotifyPropertyChanged("lastModifiedBy");
            }
        }

        private DateTime _lastModifiedDate;

        public DateTime lastModifiedDate
        {
            get { return _lastModifiedDate; }
            set
            {
                _lastModifiedDate = value;
                this.NotifyPropertyChanged("lastModifiedDate");
            }
        }

        private int _quantity;

        public int quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                this.NotifyPropertyChanged("quantity");
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

        private string _pName;

        public string pName
        {
            get { return _pName; }
            set
            {
                _pName = value;
                this.NotifyPropertyChanged("pName");
            }
        }

        private double _costPrice;

        public double costPrice
        {
            get { return _costPrice; }
            set
            {
                _costPrice = value;
                this.NotifyPropertyChanged("costPrice");
            }
        }

        public double totalCost
        {
            get
            {
                return costPrice * quantity;
            }
        }

        public double contribution
        {
            get
            {
                return totalSales - totalCost;
            }
        }


        private ObservableCollection<CartProductModel> _itemLst = new ObservableCollection<CartProductModel>();

        public ObservableCollection<CartProductModel> itemLst
        {
            get { return _itemLst; }
            set
            {
                _itemLst = value;
                this.NotifyPropertyChanged("itemLst");
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
