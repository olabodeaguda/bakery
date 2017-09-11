using BakeryPR.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ProductionProduct : INotifyPropertyChanged
    {

        public ProductDao productDao
        {
            get
            {
                return new ProductDao();
            }
        }

        private string _weightMsg = "Product Weight(Kg)";

        public string weightMsg
        {
            get { return _weightMsg; }
            set
            {
                _weightMsg = value;
                this.NotifyPropertyChanged("weightMsg");
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

        private int _productId = -1;

        public int productId
        {
            get { return _productId; }
            set
            {
                if (_productId != value)
                {
                    Product pr = productDao.byId(value);
                    weight = pr.weight;
                    if (pr.measureTypeName.ToLower() == "kg")
                    {
                        weightMsg = "Product Weight(Kg)";
                    }
                    else
                    {
                        weightMsg = "Product Weight(Gram)";
                    }
                }
                _productId = value;
                this.NotifyPropertyChanged("productId");
            }
        }

        private int _productionId = -1;

        public int productionId
        {
            get { return _productionId; }
            set
            {
                _productionId = value;
                this.NotifyPropertyChanged("productionId");
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

        private int _expectedQuantity = 0;

        public int expectedQuantity
        {
            get { return _expectedQuantity; }
            set
            {
                _expectedQuantity = value;
                this.NotifyPropertyChanged("expectedQuantity");
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

        public string totalWeight
        {
            get
            {
                return $"{(weight * quantity)}{measureTypeName}";
            }
        }

        private double _overheadCost;

        public double overheadCost
        {
            get { return _overheadCost; }
            set
            {
                _overheadCost = value;
                this.NotifyPropertyChanged("overheadCost");
            }
        }

        private double _ingredientCost;

        public double ingredientCost
        {
            get { return _ingredientCost; }
            set
            {
                _ingredientCost = value;
                this.NotifyPropertyChanged("ingredientCost");
            }
        }

        public double totalCost
        {
            get { return this.overheadCost + this.ingredientCost + this.costOfPackage; }
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
