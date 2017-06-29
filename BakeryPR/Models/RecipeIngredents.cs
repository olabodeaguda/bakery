using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class RecipeIngredents : INotifyPropertyChanged
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

        private int _recipeid;

        public int recipeId
        {
            get { return _recipeid; }
            set
            {
                _recipeid = value;
                this.NotifyPropertyChanged("recipeId");
            }
        }

        private int _ingredentId;

        public int ingredentId
        {
            get { return _ingredentId; }
            set
            {
                _ingredentId = value;
                this.NotifyPropertyChanged("ingredentId");
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
