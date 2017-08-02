using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class OverheadDetails : INotifyPropertyChanged
    {
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

        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _groupName;

        public string groupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                this.NotifyPropertyChanged("groupName");
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


        private int _overheadId;

        public int overheadId
        {
            get { return _overheadId; }
            set
            {
                _overheadId = value;
                this.NotifyPropertyChanged("overheadId");
            }
        }

        private double _overheadQuantity;

        public double overheadQuantity
        {
            get { return _overheadQuantity; }
            set
            {
                _overheadQuantity = value;
                this.NotifyPropertyChanged("overheadQuantity");
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
