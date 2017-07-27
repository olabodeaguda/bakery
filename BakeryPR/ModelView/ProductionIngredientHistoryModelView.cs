using BakeryPR.DAO;
using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.ModelView
{
    public class ProductionIngredientHistoryModelView : INotifyPropertyChanged
    {

        private int _selectIngredent;

        public int selectedIngredent
        {
            get { return _selectIngredent; }
            set
            {
                _selectIngredent = value;
                this.inventoryHistory = new ObservableCollection<InventoryHistory>(inventoryDao.byId(_selectIngredent));
                this.NotifyPropertyChanged("selectedIngredent");
            }
        }


        public IngredentDao ingredentDao
        {
            get
            {
                return new IngredentDao();
            }
        }

        public InventoryHistoryDao inventoryDao
        {
            get
            {
                return new InventoryHistoryDao();
            }
        }

        public List<Ingredent> Ingredients
        {
            get
            {
                return this.ingredentDao.all();
            }
        }

        private ObservableCollection<InventoryHistory> _inventoryHistory = new ObservableCollection<InventoryHistory>();

        public ObservableCollection<InventoryHistory> inventoryHistory
        {
            get { return _inventoryHistory; }
            set
            {
                _inventoryHistory = value;
                this.NotifyPropertyChanged("inventoryHistory");
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
