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
        public DelegateCommand<object> loadEditProductionCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    EditProduction prod = new EditProduction();
                    this.production = (Production)s;
                    prod.DataContext = this;
                    prod.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> EditProductionCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            Error checkResource = rdao.checkRecipeQuantity(this.production.recipeId,this.production.quantity);

                            if (checkResource.success)
                            {
                                bool sd = dao.update(this.production);
                                if (sd)
                                {
                                    MessageBox.Show("Production have been updated successfully");
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
                });
            }
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddProduction prod = new AddProduction();
                    production = new Production();
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
                        if(this.production.quantity == 0)
                        {
                            throw new Exception("Qantity can't be zero");
                        }
                        Error checkResource = rdao.checkRecipeQuantity(this.production.recipeId,this.production.quantity);

                        if (checkResource.success)
                        {
                            // check if it already exist
                            this.production.title = this.production.title + DateTime.Now.ToString("yyyy-MM-dd");
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

        public DelegateCommand<object> loadupdateProdOverhead
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    this.prodOverhead = (ProductionOverhead)s;
                    EditProductionOverhead editPoverhead = new EditProductionOverhead();
                    editPoverhead.DataContext = this;
                    editPoverhead.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> updateProdOverhead
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    this.isSpin = Visibility.Visible;
                    await Task.Run(() =>
                    {
                        try
                        {
                            bool d = prodDao.update(this.prodOverhead);
                            if (d)
                            {
                                MessageBox.Show("Saved");
                                this.prodOverheads = new ObservableCollection<ProductionOverhead>(prodDao.byproductionId(this.prodOverhead.productionId));
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
                return new DelegateCommand<object>(async (s) =>
                {
                    this.isSpin = Visibility.Visible;
                    await Task.Run(() =>
                    {
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

                            // check if overhead already exist

                            this.prodOverhead.productionId = this.production.id;
                            ProductionOverhead pover = prodDao.byproductionOverheadId(this.prodOverhead.productionId, this.prodOverhead.overheadId);

                            if (pover == null)
                            {
                                bool d = prodDao.add(this.prodOverhead);
                                if (d)
                                {
                                    MessageBox.Show("Saved");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Selected overhead already exist");
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message);
                        }
                    });

                    this.prodOverheads = new ObservableCollection<ProductionOverhead>(prodDao.byproductionId(this.production.id));

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

        private ObservableCollection<ProductionOverhead> _prodOverheads = new ObservableCollection<ProductionOverhead>();

        public ObservableCollection<ProductionOverhead> prodOverheads
        {
            get { return new ObservableCollection<ProductionOverhead>(prodDao.byproductionId(this.production.id)); }
            set
            {
                _prodOverheads = value;
                this.NotifyPropertyChanged("prodOverheads");
            }
        }

        #endregion

        #region product

        public DelegateCommand<object> loadUpdateProdproduct
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    EditProductionProduct epp = new EditProductionProduct();
                    this.productionProduct = (ProductionProduct)s;
                    epp.DataContext = this;
                    epp.ShowDialog();

                    /* await Task.Run(() =>
                     {
                         try
                         {
                             EditProductionProduct epp = new EditProductionProduct();
                             this.productionProduct = (ProductionProduct)s;
                             //epp.DataContext = this;
                             epp.ShowDialog();
                         }
                         catch (Exception x)
                         {
                             MessageBox.Show(x.Message);
                         }
                     });*/
                });
            }
        }

        public DelegateCommand<object> UpdateProdproduct
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                     {
                         try
                         {
                             this.isSpin = Visibility.Visible;
                             bool result = pProductDao.update(this.productionProduct);
                             if (result)
                             {
                                 this.isSpin = Visibility.Collapsed;
                                 MessageBox.Show("saved");
                                 this.productionProducts = new ObservableCollection<ProductionProduct>(pProductDao.byproductionId(this.productionProduct.productionId));
                             }
                             else
                             {
                                 MessageBox.Show("Please try again or contact adnministrator");
                             }

                         }
                         catch (Exception x)
                         {
                             this.isSpin = Visibility.Collapsed;
                             MessageBox.Show(x.Message);
                         }
                     });
                });
            }
        }

        public DelegateCommand<object> addPProductCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (production.id != -1)
                            {
                                this.productionProduct.productionId = production.id;

                                ProductionProduct pp = this.pProductDao.byProductionproductId(this.productionProduct.productionId,
                                    this.productionProduct.productId);

                                // check if the available gram is not more than the recipe total gram
                                if (pp == null)
                                {
                                    bool result = this.pProductDao.add(this.productionProduct);
                                    if (result)
                                    {
                                        MessageBox.Show("Product have been added successfully");
                                        this.productionProducts = new ObservableCollection<ProductionProduct>(pProductDao.byproductionId(this.productionProduct.productionId));
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Selected product already exist for the selected production");
                                }
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message);
                        }
                    });
                });
            }
        }

        public DelegateCommand<object> loadProductCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddProductionProduct prod = new AddProductionProduct();
                    productionProduct = new ProductionProduct();
                    this.production = (Production)s;
                    prod.DataContext = this;
                    prod.ShowDialog();
                });
            }
        }

        private ProductionProduct _productionProduct = new ProductionProduct();

        public ProductionProduct productionProduct
        {
            get { return _productionProduct; }
            set
            {
                _productionProduct = value;
                this.NotifyPropertyChanged("productionProduct");
            }
        }


        public ProductDao productDao
        {
            get
            {
                return new ProductDao();
            }
        }

        public ProductionProductDao pProductDao
        {
            get
            {
                return new ProductionProductDao();
            }
        }

        private ObservableCollection<Product> _products = new ObservableCollection<Product>();

        public ObservableCollection<Product> products
        {
            get { return new ObservableCollection<Product>(productDao.all()); }
            set
            {
                _products = value;
                this.NotifyPropertyChanged("products");
            }
        }

        private ObservableCollection<ProductionProduct> _productionproducts = new ObservableCollection<ProductionProduct>();

        public ObservableCollection<ProductionProduct> productionProducts
        {
            get
            {
                if (production.id != -1)
                {
                    var t = pProductDao.byproductionId(production.id);
                    _productionproducts = new ObservableCollection<ProductionProduct>(t);
                }

                return _productionproducts;
            }
            set
            {
                _productionproducts = value;
                this.NotifyPropertyChanged("productionProducts");
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
