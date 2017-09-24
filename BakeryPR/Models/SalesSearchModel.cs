using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class SalesSearchModel : INotifyPropertyChanged
    {
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

        private DateTime _salesDate= DateTime.Now;

        public DateTime salesDate
        {
            get { return _salesDate; }
            set
            {
                _salesDate = value;
                this.NotifyPropertyChanged("salesDate");
            }
        }

        private string _salesDateDisplay = DateTime.Now.ToShortDateString();

        public string salesDateDisplay
        {
            get { return _salesDateDisplay; }
            set
            {
                _salesDateDisplay = value;
                this.NotifyPropertyChanged("salesDateDisplay");
            }
        }

        private DateTime _salesEndDate = DateTime.Now;

        public DateTime salesEndDate
        {
            get { return _salesEndDate; }
            set
            {
                _salesEndDate = value;
                this.NotifyPropertyChanged("salesEndDate");
            }
        }

        private string _salesDateEndDisplay = DateTime.Now.ToShortDateString();

        public string salesDateEndDisplay
        {
            get { return _salesDateEndDisplay; }
            set
            {
                _salesDateEndDisplay = value;
                this.NotifyPropertyChanged("salesDateEndDisplay");
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
