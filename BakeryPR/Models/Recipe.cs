using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class Recipe : INotifyPropertyChanged
    {
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

        private ObservableCollection<RecipeIngredents> _ingredent = new ObservableCollection<RecipeIngredents>();

        public ObservableCollection<RecipeIngredents> ingredent
        {
            get { return _ingredent; }
            set
            {
                _ingredent = value;
                this.NotifyPropertyChanged("ingredent");
            }
        }

        public int ingredentNos
        {
            get { return this.ingredent.Count; }
        }

        public double totalCost
        {
            get { return Math.Round(this.ingredent.Sum(x => (x.quantity * x.unitCost)), 2); }
        }

        public string totalDoughWeight
        {
            get
            {
                return $"{Math.Round(this.ingredent.Sum(x => x.quantity), 2)} kg";
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
