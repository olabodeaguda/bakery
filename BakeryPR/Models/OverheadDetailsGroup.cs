﻿using System;
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
