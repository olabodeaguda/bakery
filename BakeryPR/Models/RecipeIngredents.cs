using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class RecipeIngredents : INotifyPropertyChanged
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

        public string quantityView
        {
            get
            {
                return this.quantity.ToString() + "" + this.mType;
            }

        }

        private int _recipeid;

        public int recipeId
        {
            get { return _recipeid; }
            set
            {
                _recipeid = value;
                this.NotifyPropertyChanged("recipeId");
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

        private string _mType;

        public string mType
        {
            get { return _mType; }
            set
            {
                _mType = value;
                this.NotifyPropertyChanged("mType");
            }
        }

        public double totalCost
        {
            get { return (this.unitCost * this.quantity); }
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
