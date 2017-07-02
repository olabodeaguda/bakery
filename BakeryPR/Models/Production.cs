using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class Production : INotifyPropertyChanged
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _title;

        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.NotifyPropertyChanged("title");
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

        private DateTime _lastUpdated;

        public DateTime lastUpdated
        {
            get { return _lastUpdated; }
            set
            {
                _lastUpdated = value;
                this.NotifyPropertyChanged("lastUpdated");
            }
        }

        private string _createdBY;

        public string createdBY
        {
            get { return _createdBY; }
            set
            {
                _createdBY = value;
                this.NotifyPropertyChanged("createdBY");
            }
        }
        private int _productCount;

        public int productCount
        {
            get { return _productCount; }
            set
            {
                _productCount = value;
                this.NotifyPropertyChanged("productCount");
            }
        }

        private int _productionCount;

        public int productionCount
        {
            get { return _productionCount; }
            set
            {
                _productionCount = value;
                this.NotifyPropertyChanged("productionCount");
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
