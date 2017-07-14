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

        public RecipeIngredentDao riDao
        {
            get
            {
                return new RecipeIngredentDao();
            }
        }

        public RecipeDao dao
        {
            get
            {
                return new RecipeDao();
            }
        }

        public IngredentDao ingredentDao
        {
            get
            {
                return new IngredentDao();
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
                    this.recipes = new ObservableCollection<Recipe>(dao.all());
                });
            }
        }

        public DelegateCommand<object> addIngredntCommmand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        if (this.recipe == null)
                        {
                            throw new Exception("Recipe not available");
                        }

                        this.recipeIngredent.recipeId = this.recipe.id;

                        // check if combination already exit

                        RecipeIngredents rri = this.riDao.byRecipeIdIngredent(this.recipeIngredent.recipeId, this.recipeIngredent.ingredentId);
                        if (rri != null)
                        {
                            MessageBox.Show("recipe already exit");
                            return;
                        }

                        bool result = this.riDao.add(this.recipeIngredent);
                        if (result)
                        {
                            MessageBox.Show("saved");
                            this.recipeIngredent = new RecipeIngredents();
                            this.riIngredents = new ObservableCollection<RecipeIngredents>(riDao.byRecipeId(this.recipe.id));
                        }
                        else
                        {
                            MessageBox.Show("an error occur while saving your request");
                        }

                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
            }
        }

        public DelegateCommand<object> loadAddIngredntCommmand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddRecipeIngredent rep = new AddRecipeIngredent();
                    rep.DataContext = this;
                    rep.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> addCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
               {
                   await Task.Run(() =>
                   {
                       try
                       {
                           Recipe ig = this.recipe;
                           double quan = 0;
                           if (!double.TryParse(this.recipe.quantity.ToString(), out quan))
                           {
                               throw new Exception("Quantity is in the wrong format");
                           }

                           if (quan == 0)
                           {
                               throw new Exception("Quantity can't be zero");
                           }

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

               });
            }
        }

        private RecipeIngredents _recipeIngredent = new RecipeIngredents();

        public RecipeIngredents recipeIngredent
        {
            get { return _recipeIngredent; }
            set
            {
                _recipeIngredent = value;
                this.NotifyPropertyChanged("recipeIngredent");
            }
        }

        private ObservableCollection<RecipeIngredents> _riIngredents = new ObservableCollection<RecipeIngredents>();

        public ObservableCollection<RecipeIngredents> riIngredents
        {
            get
            {
                return _riIngredents;
            }
            set
            {
                _riIngredents = value;
                this.NotifyPropertyChanged("riIngredents");
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

        public ObservableCollection<Ingredent> ingredents
        {
            get
            {
                List<Ingredent> lst = new List<Ingredent>()
                {
                    new Ingredent() { id = -1, ingredentName = "none" }
                };

                lst.AddRange(ingredentDao.all());

                return new ObservableCollection<Ingredent>(lst);
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

        #region edit


        public DelegateCommand<object> loadEditCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    if (s != null)
                    {
                        EditRecipe editrecipe = new EditRecipe();
                        editrecipe.DataContext = this;
                        this.recipe = (Recipe)s;
                        this.riIngredents = new ObservableCollection<RecipeIngredents>(riDao.byRecipeId(this.recipe.id));
                        bool? result = editrecipe.ShowDialog();
                        this.recipes = new ObservableCollection<Recipe>(dao.all());
                    }
                });
            }
        }

        public DelegateCommand<object> loadEditIngredentCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    if (s != null)
                    {
                        EditRecipeIngredent editRecipe = new EditRecipeIngredent();
                        this.recipeIngredent = (RecipeIngredents)s;
                        editRecipe.DataContext = this;
                        editRecipe.ShowDialog();
                    }
                });
            }
        }

        public DelegateCommand<object> updateCommand
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

                        bool result = dao.update(ig);
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

        public DelegateCommand<object> updateIngredntCommmand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        bool result = this.riDao.Update(this.recipeIngredent);
                        if (result)
                        {
                            MessageBox.Show("saved");
                            this.riIngredents = new ObservableCollection<RecipeIngredents>(riDao.byRecipeId(this.recipe.id));
                        }
                        else
                        {
                            MessageBox.Show("an error occur while saving your request");
                        }

                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                });
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
