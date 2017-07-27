using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ItemModel : INotifyPropertyChanged
    {
        private int _proudctId;

        public int proudctId
        {
            get { return _proudctId; }
            set
            {
                _proudctId = value;
                this.NotifyPropertyChanged("proudctId");
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
