using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class Ingredent : INotifyPropertyChanged
    {
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

        private int _mTypeId;

        public int mTypeId
        {
            get { return _mTypeId; }
            set
            {
                _mTypeId = value;
                this.NotifyPropertyChanged("mTypeId");
            }
        }


        private double _quantity;

        public double quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                this.NotifyPropertyChanged("quantity");
            }
        }

        public string quantityExpression
        {
            get
            {
                return $"{ this.quantity} {this.measureTypeName}";
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

        private double _unitCost;

        public double unitCost
        {
            get { return _unitCost; }
            set
            {
                _unitCost = value;
                this.NotifyPropertyChanged("unitCost");
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

        public string ingredentNameDisplay
        {
            get
            {
                if (ingredentName.ToLower() == "none")
                {
                    return ingredentName;
                }
                else
                {
                    return $"{ingredentName} (in {measureTypeName})";
                }
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
