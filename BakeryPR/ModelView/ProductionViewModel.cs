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
