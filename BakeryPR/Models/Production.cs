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

        private int _id = -1;

        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.NotifyPropertyChanged("id");
            }
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

        private string _recipeTitle;

        public string recipeTitle
        {
            get { return _recipeTitle; }
            set
            {
                _recipeTitle = value;
                this.NotifyPropertyChanged("recipeTitle");
            }
        }

        private int _recipeId = -1;

        public int recipeId
        {
            get { return _recipeId; }
            set
            {
                _recipeId = value;
                this.NotifyPropertyChanged("recipeId");
            }
        }

        private string _approval;

        public string approval
        {
            get { return _approval; }
            set
            {
                _approval = value;
                this.NotifyPropertyChanged("approval");
            }
        }

        private string _approveBy;

        public string approveBy
        {
            get { return _approveBy; }
            set
            {
                _approveBy = value;
                this.NotifyPropertyChanged("approveBy");
            }
        }

        private int _overheadGrpId = -1;

        public int overheadGrpId
        {
            get { return _overheadGrpId; }
            set
            {
                _overheadGrpId = value;
                this.NotifyPropertyChanged("overheadGrpId");
            }
        }

        private long _dateCreatedTimespan;

        public long dateCreatedTimespan
        {
            get { return _dateCreatedTimespan; }
            set
            {
                _dateCreatedTimespan = value;
                this.NotifyPropertyChanged("dateCreatedTimespan");
            }
        }

        private double _dateCreateTSpan;

        public double dateCreateTSpan
        {
            get { return _dateCreateTSpan; }
            set
            {
                _dateCreateTSpan = value;
                //dateCreatedTimespan =  long.Parse(value);
                this.NotifyPropertyChanged("dateCreateTSpan");
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
