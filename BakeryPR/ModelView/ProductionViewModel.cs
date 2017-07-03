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
    public class ProductionViewModel : INotifyPropertyChanged
    {
        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddProduction prod = new AddProduction();
                    prod.DataContext = this;
                    prod.ShowDialog();
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
                        Error checkResource = rdao.checkRecipeQuantity(this.production.recipeId);

                        if (checkResource.success)
                        {
                            bool sd = dao.add(this.production);
                            if (sd)
                            {
                                MessageBox.Show("Production have been added successfully");
                                this.production = new Production();
                                this.productions = new ObservableCollection<Production>(dao.all());
                            }
                        }
                        else
                        {
                            MessageBox.Show(checkResource.errorMsg);
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
            }
        }


        public ProductionDao dao
        {
            get
            {
                return new ProductionDao();
            }
        }

        public RecipeDao rdao
        {
            get
            {
                return new RecipeDao();
            }
        }

        private ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>();

        public ObservableCollection<Recipe> recipes
        {
            get
            {
                List<Recipe> lst = new List<Recipe>() { new Recipe() { id = -1, title = "none" } };
                lst.AddRange(this.rdao.all());
                _recipes = new ObservableCollection<Recipe>(lst);
                return _recipes;
            }
            set
            {
                _recipes = value;
                this.NotifyPropertyChanged("recipes");
            }
        }


        private Production _production = new Production();

        public Production production
        {
            get { return _production; }
            set
            {
                _production = value;
                this.NotifyPropertyChanged("production");
            }
        }


        private ObservableCollection<Models.Production> _productions;

        public ObservableCollection<Production> productions
        {
            get
            {
                _productions = new ObservableCollection<Production>(dao.all());
                return _productions;
            }
            set
            {
                _productions = value;
                this.NotifyPropertyChanged("productions");
            }
        }

        private Visibility _isSpin = Visibility.Collapsed;

        public Visibility isSpin
        {
            get { return _isSpin; }
            set
            {
                _isSpin = value;

                this.NotifyPropertyChanged("isSpin");
            }
        }


        #region Overheads

        public DelegateCommand<object> loadOverheadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    this.production = (Production)s;
                    this.prodOverhead.productionId = this.production.id;
                    AddProductionOverhead prodoverhead = new AddProductionOverhead();
                    prodoverhead.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> addOverheadCommand
        {
            get
            {
                return new DelegateCommand<object>( async (s) =>
                 {
                     this.isSpin = Visibility.Visible;
                     await Task.Run(() => {
                         try
                         {
                             if (this.prodOverhead.overheadId < 1)
                             {
                                 MessageBox.Show("Please select Overhead");

                                 this.isSpin = Visibility.Collapsed;
                                 return;
                             }

                             else if (this.prodOverhead.overheadCount < 1)
                             {
                                 this.prodOverhead.overheadCount = 1;
                             }

                             this.prodOverhead.productionId = this.production.id;

                             bool d = prodDao.add(this.prodOverhead);
                             if (d)
                             {
                                 MessageBox.Show("Saved");
                             }
                         }
                         catch (Exception x)
                         {
                             MessageBox.Show(x.Message);
                         }
                     });


                     this.isSpin = Visibility.Collapsed;
                 });
            }
        }

        public ProductionOverheadDao prodDao
        {
            get
            {
                return new ProductionOverheadDao();
            }
        }

        private ProductionOverhead _prodOverhead = new ProductionOverhead();

        public ProductionOverhead prodOverhead
        {
            get { return _prodOverhead; }
            set
            {
                _prodOverhead = value;
                this.NotifyPropertyChanged("prodOverhead");
            }
        }


        public OverheadDao overheadDao
        {
            get
            {
                return new OverheadDao();
            }
        }

        public ObservableCollection<Overhead> overheads
        {
            get
            {
                List<Overhead> lst = new List<Overhead>() { new Overhead() { id = -1, name = "none" } };
                lst.AddRange(overheadDao.all());
                return new ObservableCollection<Overhead>(lst);
            }
        }

        #endregion


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
