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

        private int _recipeNos = 0;
        public int recipeNos
        {
            get { return _recipeNos; }
            set
            {
                _recipeNos = value;
                this.NotifyPropertyChanged("recipeNos");
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
