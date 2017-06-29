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
    public class RecipeIngredentModelView : INotifyPropertyChanged
    {
        private bool _isAddIngredent;

        public bool isAddIngredent
        {
            get { return _isAddIngredent; }
            set
            {
                _isAddIngredent = value;
                this.NotifyPropertyChanged("isAddIngredent");
            }
        }


        public RecipeDao dao
        {
            get
            {
                return new RecipeDao();
            }
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddRecipe prod = new AddRecipe();
                    prod.DataContext = this;
                    prod.ShowDialog();
                    //List<Recipe> lst = dao.all();
                    //var t = lst.FirstOrDefault(x => x.title == this.recipe.title);
                    //this.recipe = t != null ? t : new Recipe();
                    //this.recipes = new ObservableCollection<Recipe>(lst);
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
                        /* EditIngredent editIngr = new EditIngredent();
                         this.selectIngredent = (Ingredent)s;

                         editIngr.DataContext = this;
                         bool? result = editIngr.ShowDialog();
                         this.Ingredents = new ObservableCollection<Ingredent>(dao.all());*/
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
                        Recipe ig = this.recipe;
                        ig.dateCreated = DateTime.Now;
                        ig.lastUpdated = DateTime.Now;

                         bool result = dao.add(ig);
                         if (result)
                         {
                             MessageBox.Show("Saved");
                         }
                         else
                         {
                             MessageBox.Show("not saved");
                         }
                        List<Recipe> lst = dao.all();
                        var t = lst.FirstOrDefault(x => x.title == ig.title);
                        this.recipe = t != null ? t : new Recipe();
                        this.recipes = new ObservableCollection<Recipe>(lst);
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
            }
        }

        private ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>();

        public ObservableCollection<Recipe> recipes
        {
            get { return new ObservableCollection<Recipe>(dao.all()); }
            set
            {
                _recipes = value;
                this.NotifyPropertyChanged("recipes");
            }
        }

        private Recipe _recipe = new Recipe();

        public Recipe recipe
        {
            get { return _recipe; }
            set
            {
                _recipe = value;
                if (_recipe.id > -1)
                {
                    this.isAddIngredent = true;
                }
                else
                {
                    this.isAddIngredent = false;
                }
                this.NotifyPropertyChanged("recipe");
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
