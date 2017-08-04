using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class OverheadDetailsGroup : INotifyPropertyChanged
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

        private int _grpId;

        public int grpId
        {
            get { return _grpId; }
            set
            {
                _grpId = value;
                this.NotifyPropertyChanged("grpId");
            }
        }

        private int _overheadId = -1;

        public int overheadId
        {
            get { return _overheadId; }
            set
            {
                _overheadId = value;
                this.NotifyPropertyChanged("overheadId");
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

        public double totalCost
        {
            get
            {
                return unitCost * quantity;
            }
        }

        public string quantityView
        {
            get
            {
                return $"{quantity}{measureType}";
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
