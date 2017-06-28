using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class Overhead : INotifyPropertyChanged
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

        private string _name;

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.NotifyPropertyChanged("name");
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
