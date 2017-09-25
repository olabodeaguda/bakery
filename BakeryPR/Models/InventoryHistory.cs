using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class InventoryHistory : INotifyPropertyChanged
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

        private int _ingredentId;

        public int ingredentId
        {
            get { return _ingredentId; }
            set
            {
                _ingredentId = value;
                this.NotifyPropertyChanged("ingredentId");
            }
        }

        private double _amount;

        public double amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                this.NotifyPropertyChanged("amount");
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

        private string _ingredentName;

        public string ingredentName
        {
            get { return _ingredentName; }
            set
            {
                _ingredentName = value;
                this.NotifyPropertyChanged("ingredentName");
            }
        }


        private string _addedBy;

        public string addedBy
        {
            get { return _addedBy; }
            set
            {
                _addedBy = value;
                this.NotifyPropertyChanged("addedBy");
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

        private string _measureType;

        public string measureType
        {
            get { return _measureType; }
            set
            {
                _measureType = value;
                this.NotifyPropertyChanged("measureType");
            }
        }

        public string amountDisplay
        {
            get
            {
                return $"{amount}{measureType}";
            }
        }

        private double _newQuantity;

        public double newQuantity
        {
            get { return _newQuantity; }
            set
            {
                _newQuantity = value;
                this.NotifyPropertyChanged("newQuantity");
            }
        }

        private double _newUnitCost;

        public double newUnitCost
        {
            get { return _newUnitCost; }
            set
            {
                _newUnitCost = value;
                this.NotifyPropertyChanged("newUnitCost");
            }
        }

        private double _oldUnitCost;

        public double oldUnitCost
        {
            get { return _oldUnitCost; }
            set
            {
                _oldUnitCost = value;
                this.NotifyPropertyChanged("oldUnitCost");
            }
        }

        private long _dateTimeSpan;

        public long dateTimeSpan
        {
            get { return _dateTimeSpan; }
            set { _dateTimeSpan = value; }
        }

        private double _replenishCount;

        public double replenishCount
        {
            get { return _replenishCount; }
            set
            {
                _replenishCount = value;
                this.NotifyPropertyChanged("replenishCount");
            }
        }

        public string replenishDisplay
        {
            get
            {
                return $"{replenishCount}{measureType}";
            }
        }

        private double _productionCount;

        public double productionCount
        {
            get { return _productionCount; }
            set
            {
                _productionCount = value;
                this.NotifyPropertyChanged("productionCount");
            }
        }

        public string productionCountDisplay
        {
            get
            {
                return $"{productionCount}{measureType}";
            }
        }

        private double _balance;

        public double balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                this.NotifyPropertyChanged("balance");
            }
        }

        public string balanceDisplay
        {
            get
            {
                return $"{balance}{measureType}";
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
