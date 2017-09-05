using BakeryPR.DAO;
using BakeryPR.Models;
using BakeryPR.Utilities;
using BakeryPR.Views;
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
    public class IngredentModelView : INotifyPropertyChanged
    {
        public void loadAddWin(object param)
        {
            AddIngredent add = new AddIngredent();
            bool? result = add.ShowDialog();
            this.Ingredents = new ObservableCollection<Ingredent>(dao.all());
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>(loadAddWin);
            }
        }

        public DelegateCommand<object> UpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        Ingredent sd = this.selectIngredent;
                        bool result = dao.update(sd);
                        if (result)
                        {
                            MessageBox.Show("Update was successfull");
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
            }
        }

        public DelegateCommand<object> loadEditCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {

                    if (s != null)
                    {
                        EditIngredent editIngr = new EditIngredent();
                        this.selectIngredent = (Ingredent)s;

                        editIngr.DataContext = this;
                        bool? result = editIngr.ShowDialog();
                        this.Ingredents = new ObservableCollection<Ingredent>(dao.all());
                    }
                });
            }
        }

        public DelegateCommand<object> loadUpdateInventoryCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {

                    if (s != null)
                    {
                        UpdateInventory inven = new UpdateInventory();

                        Ingredent sd = (Ingredent)s;

                        this.inventoryHistory = new InventoryHistory()
                        {
                            ingredentId = sd.id,
                            ingredentName = sd.ingredentName,
                            amount = sd.quantity,
                            oldUnitCost = sd.unitCost
                        };

                        inven.DataContext = this;
                        inven.ShowDialog();
                    }
                });
            }
        }

        public AppConfigDao appConfigDao
        {
            get
            {
                return new AppConfigDao();
            }
        }

        public LoginModel loginModel
        {
            get
            {
                return appConfigDao.read();
            }
        }

        public DelegateCommand<object> updateQuantityCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        Ingredent ig = this.ingredent;

                        double val = 0;
                        if (!double.TryParse(this.inventoryHistory.newQuantity.ToString(),out val))
                        {
                            throw new Exception("Invalid input for quantity");
                        }
                        else if(!double.TryParse(this.inventoryHistory.newUnitCost.ToString(), out val))
                        {
                            throw new Exception("Invalid input for Unit Cost");
                        }

                        this.inventoryHistory.inventoryMode = InventoryMode.ADD.ToString();
                        this.inventoryHistory.addedBy = loginModel.fullname;

                        bool result = invenHistoryDao.add(this.inventoryHistory);
                        if (result)
                        {
                            this.Ingredents = new ObservableCollection<Ingredent>(dao.all());
                            MessageBox.Show("Saved");
                        }
                        else
                        {
                            MessageBox.Show("not saved");
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
            }
        }

        public DelegateCommand<object> addCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        Ingredent ig = this.ingredent;

                        double val = 0;
                        if (!double.TryParse(ig.newQuantity.ToString(), out val))
                        {
                            throw new Exception("Invalid input for quantity");
                        }

                        if (ig.measureTypeName.ToLower() == "gram")
                        {
                            ig.newQuantity = ig.newQuantity / 1000;
                            ig.mTypeId = 1;
                        }

                        bool result = dao.add(ig);
                        if (result)
                        {
                            MessageBox.Show("Saved");
                        }
                        else
                        {
                            MessageBox.Show("not saved");
                        }
                        this.ingredent = new Ingredent();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
            }
        }

        private IngredentDao dao
        {
            get
            {
                return new IngredentDao();
            }
        }

        private InventoryHistoryDao invenHistoryDao
        {
            get
            {
                return new InventoryHistoryDao();
            }
        }

        private MeasureTypeDao mDao
        {
            get
            {
                return new MeasureTypeDao();
            }
        }

        private ObservableCollection<Ingredent> _Ingredents;
        public ObservableCollection<Ingredent> Ingredents
        {
            get
            {

                _Ingredents = new ObservableCollection<Ingredent>(dao.all());
                return _Ingredents;
            }
            set
            {
                _Ingredents = value;
                this.NotifyPropertyChanged("Ingredents");
            }
        }

        private Ingredent _selectIngredent = new Ingredent();
        public Ingredent selectIngredent
        {
            get
            {
                return _selectIngredent;
            }
            set
            {
                if (_selectIngredent != value)
                {
                    _selectIngredent = value;
                    this.NotifyPropertyChanged("selectIngredent");
                }
            }
        }

        private Ingredent _ingredent = new Ingredent();
        public Ingredent ingredent
        {
            get
            {
                return _ingredent;
            }
            set
            {
                if (_ingredent != value)
                {
                    _ingredent = value;
                    this.NotifyPropertyChanged("ingredent");
                }
            }
        }

        public ObservableCollection<MeasurementType> MeasurementTypes
        {
            get
            {
                List<MeasurementType> lst = new List<MeasurementType>();
                lst.Add(new MeasurementType() { id = -1, name = "none" });
                lst.AddRange(mDao.all());
                return new ObservableCollection<MeasurementType>(lst);
            }
        }

        private InventoryHistory _inventoryHistory = new InventoryHistory();

        public InventoryHistory inventoryHistory
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
