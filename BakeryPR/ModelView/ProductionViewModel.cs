﻿using BakeryPR.DAO;
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
                            Error checkResource = rdao.checkRecipeQuantity(this.production.recipeId, this.production.quantity);

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
                        if (this.production.quantity == 0)
                        {
                            throw new Exception("Quantity can't be zero");
                        }

                        List<Production> bystatuslst = dao.byStatus(ProductionStatus.NOT_APPROVED.ToString());
                        if (bystatuslst.Count > 0)
                        {
                            MessageBoxResult msg = MessageBox.Show("You still have some production pending approval. Do you still want to continue ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (msg == MessageBoxResult.No)
                            {
                                return;
                            }
                        }

                        Error checkResource = rdao.checkRecipeQuantity(this.production.recipeId, this.production.quantity);

                        if (!checkResource.success)
                        {
                            throw new Exception(checkResource.errorMsg);
                        }

                        //check if thier is an ongoing production...
                        this.production.approval = ProductionStatus.NOT_APPROVED.ToString();
                        this.production.id = dao.add(this.production);

                        //add selected ingredend from recipe 
                        bool result = rdao.addProdIngredentDB(this.production);
                        if (!result)
                        {
                            MessageBox.Show("Please Ingredent addition failed.. Please add ingredent manually");
                        }

                        MessageBox.Show("Production have been added successfully");
                        this.production = new Production();
                        this.productions = new ObservableCollection<Production>(dao.all());

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

        #region production

        public ProductionIngredentDao PIDao
        {
            get
            {
                return new ProductionIngredentDao();
            }
        }

        public DelegateCommand<object> loadDeleteprodIngredentCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            MessageBoxResult msg = MessageBox.Show("Are you sure?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (msg == MessageBoxResult.No)
                            {
                                return;
                            }
                            ProductionIngredent p = (ProductionIngredent)s;

                            bool result = PIDao.delete(p.id);

                            if (result)
                            {
                                this.productionIngredents = new ObservableCollection<ProductionIngredent>(PIDao.byProductionId(p.productionId));
                                MessageBox.Show("Deletion was successfull", "Deletion", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                throw new Exception("An error occur while creating you request. Try again or contanct your administrator");
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                });
            }
        }

        public DelegateCommand<object> loadIngredentCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    ProductionIngredentView pi = new ProductionIngredentView();
                    this.production = (Production)s;
                    this.productionName = $"Production title {this.production.title}";
                    this.productionIngredents = new ObservableCollection<ProductionIngredent>(PIDao.byProductionId(this.production.id));

                    pi.DataContext = this;
                    pi.ShowDialog();
                });
            }
        }

        private ObservableCollection<ProductionIngredent> _productionIngredents = new ObservableCollection<ProductionIngredent>();

        public ObservableCollection<ProductionIngredent> productionIngredents
        {
            get { return _productionIngredents; }
            set
            {
                _productionIngredents = value;
                this.NotifyPropertyChanged("productionIngredents");
            }
        }

        private string _productionName;

        public string productionName
        {
            get { return _productionName; }
            set
            {
                _productionName = value;
                this.NotifyPropertyChanged("productionName");
            }
        }

        public IngredentDao ingredentDao
        {
            get { return new IngredentDao(); }
        }

        public ObservableCollection<Ingredent> ingredents
        {
            get
            {
                return new ObservableCollection<Ingredent>(ingredentDao.all());
            }
        }

        private Ingredent _ingredent = new Ingredent();

        public Ingredent ingredent
        {
            get { return _ingredent; }
            set
            {
                _ingredent = value;
                this.NotifyPropertyChanged("ingredent");
            }
        }

        public DelegateCommand<object> loadAddProductionIngredient
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddProductionIngredient addp = new AddProductionIngredient();
                    addp.DataContext = this;
                    addp.Show();
                });
            }
        }

        public DelegateCommand<object> AddProductionIngredientCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {

                        try
                        {
                            isSpin = Visibility.Visible;
                            //check if ingredent already exist for the production
                            if (this.PIDao.byProductionIngredent(this.production.id, this.ingredent.id))
                            {
                                throw new Exception("Selected ingredent already exist");
                            }

                            //checked if amount is available
                            var ru = ingredents.FirstOrDefault(x => x.id == this.ingredent.id);
                            if (this.ingredent.quantity > ru.quantity)
                            {
                                throw new Exception("Amount og ingredent left in stock is " + ru.quantity);
                            }

                            ProductionIngredent pi = new ProductionIngredent()
                            {
                                amount = this.ingredent.quantity,
                                ingredentId = this.ingredent.id,
                                productionId = this.production.id,
                            };

                            bool res = PIDao.add(pi);
                            if (res)
                            {
                                this.productionIngredents = new ObservableCollection<ProductionIngredent>(PIDao.byProductionId(pi.productionId));
                                MessageBox.Show("Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                throw new Exception("An error occur while trying to process the request.. try again, or contact your administrator");
                            }

                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        isSpin = Visibility.Collapsed;
                    });
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

        public DelegateCommand<object> approveProductionCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            MessageBoxResult msg = MessageBox.Show("Are you sure", "Approve Production", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (msg == MessageBoxResult.No)
                            {
                                return;
                            }

                            //check if total ingredent is in storage




                            Production p = (Production)s;
                            var ru = this.appConfigDao.read();
                            bool result = PIDao.changeProdApprovalStatus(ProductionStatus.APPROVED.ToString(),
                                p.id, ru.username);

                            if (result)
                            {
                                // update product

                                this.productions = new ObservableCollection<Production>(dao.all());
                                MessageBox.Show("Approval was successfull", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                throw new Exception("Update was not successfull. Please try again or contact an administrator");
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
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
