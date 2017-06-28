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
                return new DelegateCommand<object>((s)=> {
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
                return new DelegateCommand<object>((s) => {

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

        public DelegateCommand<object> addCommand
        {
            get
            {
                return new DelegateCommand<object>((s)=> {
                    try
                    {
                        Ingredent ig = this.ingredent;

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
