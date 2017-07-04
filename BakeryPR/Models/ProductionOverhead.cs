using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ProductionOverhead : INotifyPropertyChanged
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _overheadId = -1;

        public int overheadId
        {
            get { return _overheadId; }
            set { _overheadId = value; }
        }

        private int _productionId;

        public int productionId
        {
            get { return _productionId; }
            set { _productionId = value; }
        }

        private int _overheadCount;

        public int overheadCount
        {
            get { return _overheadCount; }
            set
            {
                _overheadCount = value;
                this.NotifyPropertyChanged("overheadCount");
            }
        }

        private string _overheadName;

        public string overheadName
        {
            get { return _overheadName; }
            set
            {
                _overheadName = value;
                this.NotifyPropertyChanged("overheadName");
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

        private string _totalCost;

        public string totalCost
        {
            get
            {
                double tc = overheadCount * unitCost;
                return $"{(tc)} {this.mType}";
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
