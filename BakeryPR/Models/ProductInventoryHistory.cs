using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.Models
{
    public class ProductInventoryHistory : INotifyPropertyChanged
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

        public int index { get; set; }

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
        private DateTime _createdDate;

        public DateTime createdDate
        {
            get { return _createdDate; }
            set
            {
                _createdDate = value;
                this.NotifyPropertyChanged("_createdDate");
            }
        }

        private string _inventoryMode;

        public string inventoryMode
        {
            get { return _inventoryMode; }
            set
            {
                _inventoryMode = value;
                this.NotifyPropertyChanged("inventoryMode");
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


        private long _dateCreatedTimespan;

        public long dateCreatedTimespan
        {
            get { return _dateCreatedTimespan; }
            set
            {
                _dateCreatedTimespan = value;
                this.NotifyPropertyChanged("dateCreatedTimespan");
            }
        }


        public int quantityProduced { get; set; }

        public int quantitySold { get; set; }

        public int balance { get; set; }

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
