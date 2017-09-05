using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ProductionIngredent : INotifyPropertyChanged
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

        private int _ingredentId = -1;

        public int ingredentId
        {
            get { return _ingredentId; }
            set
            {
                _ingredentId = value;
                this.NotifyPropertyChanged("ingredentId");
            }
        }

        private int _productionId;

        public int productionId
        {
            get { return _productionId; }
            set
            {
                _productionId = value;
                this.NotifyPropertyChanged("productionId");
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

        private string _lastModifiedBy;

        public string lastModifiiedBy
        {
            get { return _lastModifiedBy; }
            set
            {
                _lastModifiedBy = value;
                this.NotifyPropertyChanged("lastModifiiedBy");
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

        private double _unitCost;

        public double unitCost
        {
            get { return _unitCost; }
            set
            {
                _unitCost = value;
                this.NotifyPropertyChanged("uniCost");
            }
        }

        public string displayQuantity
        {
            get { return $"{ this.amount}{this.measureType}"; }
        }

        public string totalUnitCost
        {
            get
            {
                return $"{Math.Round(this.amount * unitCost,2)}";
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
