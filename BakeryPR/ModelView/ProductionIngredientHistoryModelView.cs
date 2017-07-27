using BakeryPR.DAO;
using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.ModelView
{
    public class ProductionIngredientHistoryModelView : INotifyPropertyChanged
    {
        public IngredentDao ingredentDao
        {
            get
            {
                return new IngredentDao();
            }
        }

        public List<Ingredent> Ingredient
        {
            get
            {
                return this.ingredentDao.all();
            }
        }


        #region property change

        public double winHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight - 100;
            }
        }

        public double winWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth;
            }
        }

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
