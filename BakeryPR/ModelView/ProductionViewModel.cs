﻿using BakeryPR.DAO;
using BakeryPR.Models;
using BakeryPR.Utilities;
using BakeryPR.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.ModelView
{
    public class ProductionViewModel : INotifyPropertyChanged
    {
        public string title
        {
            get
            {
                return companyDetailDao.Title();
            }
        }

        public CompanyDetailDao companyDetailDao
        {
            get
            {
                return new CompanyDetailDao();
            }
        }
        private Visibility _isBusyVisible = Visibility.Collapsed;

        public Visibility isBusyVisible
        {
            get { return _isBusyVisible; }
            set
            {
                _isBusyVisible = value;
                this.NotifyPropertyChanged("isBusyVisible");
            }
        }

        public ExcelUtils ExcelU
        {
            get
            {
                return new ExcelUtils();
            }
        }

        private string _filename;

        public string filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                this.NotifyPropertyChanged("filename");
            }
        }

        public CartDao cartDao
        {
            get
            {
                return new CartDao();
            }
        }

        public DelegateCommand<object> reportCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    try
                    {
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                        dlg.DefaultExt = ".pdf";
                        dlg.Filter = "PDF files|*.pdf;";

                        Nullable<bool> result = dlg.ShowDialog();

                        if (result == true)
                        {
                            isBusyVisible = Visibility.Visible;
                            filename = dlg.FileName;
                        }
                        else
                        {
                            return;
                        }

                        Production p = (Production)s;
                        await Task.Run(async () =>
                        {
                            var pingredent = PIDao.byProductionId(p.id);
                            string ingreTemp = "<table style='border-width: 0.5px;border-style: solid;border-color: black;'>";
                            ingreTemp = ingreTemp + "<tr style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>";
                            ingreTemp = ingreTemp + "<th style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>INGREDIENT NAME</th>";
                            ingreTemp = ingreTemp + "<th style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>QUANTITY</th>";
                            ingreTemp = ingreTemp + "<th style='text-align:center;border-width:0.5px;border-style: solid;border-color: black;'>UNIT COST</th>";
                            ingreTemp = ingreTemp + "<th style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>TOTAL COST</th>";
                            ingreTemp = ingreTemp + "</tr>";

                            foreach (var tm in pingredent)
                            {
                                ingreTemp = ingreTemp + "<tr style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>";
                                ingreTemp = ingreTemp + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>{tm.ingredentName}</td>";
                                ingreTemp = ingreTemp + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>{tm.amount}{tm.measureType}</td>";
                                ingreTemp = ingreTemp + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>N{string.Format(CultureInfo.InvariantCulture, "{0:N0}", tm.unitCost)}/{tm.measureType}</td>";
                                ingreTemp = ingreTemp + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>N{string.Format(CultureInfo.InvariantCulture, "{0:N0}", tm.totalUnitCost)}</td>";
                                ingreTemp = ingreTemp + $"</tr>";
                            }
                            ingreTemp = ingreTemp + "</table>";

                            List<SalesRevenueModel> salesRM = cartDao.byDaily(p.dateCreated.ToString("yyyy-MM-dd"))
                            .Select(x => new SalesRevenueModel()
                            {
                                ProductName = x.pName,
                                QuantityProduced = x.quantity,
                                UnitPrice = x.salesType == SalesType.RETAIL.ToString() ? x.retailPrice : x.wholeSales,
                                totalPrice = x.price
                            }).ToList();

                            string template = pDFReader.GetproductionReporttemplate();
                            template = template.Replace("{{productionName}}", p.title.ToUpper());
                            template = template.Replace("{{recipeName}}", p.recipeTitle.ToUpper());
                            template = template.Replace("{{companyName}}", title.ToUpper());
                            template = template.Replace("{{date}}", p.dateCreated.ToString("dd-MM-yyyy"));
                            template = template.Replace("{{productList}}", ingreTemp);
                            template = template.Replace("{{bulkDoughWeight}}", $"{pingredent.Sum(x => x.amount)}");
                            double tw = pingredent.Sum(x => (x.unitCost * x.amount)) / pingredent.Sum(x => x.amount);
                            template = template.Replace("{{unitWeight}}", $"{ string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Math.Round(tw, 2))}");
                            template = template.Replace("{{TotalPrice}}",
                                $"{ string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Math.Round(pingredent.Sum(x => (x.unitCost * x.amount)), 2))}");

                            var pOverhead = prodDao.byproductionId(p.id);
                            string overh = "<table style='border-width: 0.5px;border-style: solid;border-color: black;'><tr style='text-align:center;border-width: 1px;border-style: solid;border-color: black;'><th style='text-align:center;border-width: 1px;border-style: solid;border-color: black;'>OVERHEAD</th><th style='text-align:center;border-width: 1px;border-style: solid;border-color: black;'>AMOUNT</th></tr>";

                            foreach (var tm in pOverhead)
                            {
                                overh = overh + $"<tr><td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>{tm.overheadName}</td>" +
                                $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>{string.Format(CultureInfo.InvariantCulture, "N{0:0,0.00}", Math.Round(tm.overheadCount), 2)}</td></tr>";
                            }
                            overh = overh + $"<tr><td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>Total </td>" +
                            $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'>{string.Format(CultureInfo.InvariantCulture, "N{0:0,0.00}", Math.Round(pOverhead.Sum(x => x.overheadCount)), 2)}</td></tr>";
                            overh = overh + "</table>";

                            template = template.Replace("{{OverheadList}}", overh);

                            var t = pProductDao.byproductionId(p.id);
                            string pducts = "";
                            foreach (var tm in pProductDao.byproductionId(p.id))
                            {
                                pducts = pducts + "<tr>";
                                pducts = pducts + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'> {tm.productName} </td>";
                                pducts = pducts + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'> {tm.quantity}</td>";
                                pducts = pducts + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'> {String.Format(CultureInfo.InvariantCulture, "N{0:0,0.00}", tm.ingredientCost)} </td>";
                                pducts = pducts + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'> {String.Format(CultureInfo.InvariantCulture, "N{0:0,0.00}",tm.overheadCost)} </td>";
                                pducts = pducts + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'> {String.Format(CultureInfo.InvariantCulture, "N{0:0,0.00}",tm.costOfPackage)} </td>";
                                pducts = pducts + $"<td style='text-align:center;border-width: 0.5px;border-style: solid;border-color: black;'> {String.Format(CultureInfo.InvariantCulture, "N{0:0,0.00}",tm.totalCost)} </td>";
                                pducts = pducts + "</tr>";
                            }

                            template = template.Replace("{{products}}",pducts);
                            await pDFReader.GetProductionReport(template, filename);

                            MessageBox.Show("Report has been generated was successfully");

                            this.isBusyVisible = Visibility.Collapsed;
                        });
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                });
            }
        }

        public PDFReader pDFReader
        {
            get
            {
                return new PDFReader();
            }
        }

        public DelegateCommand<object> loadEditProductionCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {

                        EditProduction prod = new EditProduction();
                        this.production = (Production)s;
                        this.checkvalidation();
                        prod.DataContext = this;
                        prod.ShowDialog();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                            if (this.production.quantity <= 0)
                            {
                                throw new Exception("Quantity has been inputted wrongly");
                            }
                            Error checkResource = rdao.checkRecipeQuantity(this.production.recipeId, this.production.quantity);

                            if (checkResource.success)
                            {
                                Production oldP = dao.ProductionId(this.production.id);
                                bool sd = false;
                                string query = dao.updateProductionQuery(this.production);
                                query = query + dao.deleteProductionQuery(this.production);
                                query = query + overheadDao.deleteProductionQuery(this.production);
                                query = query + rdao.addProdIngredentDBQuery(this.production);
                                query = query + ovDetilsDao.ProductionInsert(this.production.quantity, production.id);
                                query = query + pProductDao.deleteProductionQuery(this.production);
                                sd = dao.exec(query);

                                if (sd)
                                {
                                    MessageBox.Show("Production has been updated successfully");
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

        public List<OverheadDetails> overheadGrp
        {
            get
            {
                return overheadDetailDao.allSingle();
            }
        }

        public OverheadDetailsDao overheadDetailDao
        {
            get
            {
                return new OverheadDetailsDao();
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
                        if (this.production.quantity <= 0)
                        {
                            throw new Exception("Quantity of flour has been wrongly inputted");
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

                        //get registered overhead
                        //create a string insertion
                        bool result = false;

                        string ovQuery = ovDetilsDao.ProductionInsert(this.production.quantity, production.id);


                        result = rdao.addProduction(this.production, ovQuery);

                        //add selected ingredend from recipe 

                        if (!result)
                        {
                            MessageBox.Show("Please Ingredient addition failed.. Please add ingredient manually");
                        }

                        MessageBox.Show("Production has been added successfully");
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

        public OverheadDetailsGroupDao ovDetilsDao
        {
            get
            {
                return new OverheadDetailsGroupDao();
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
                    try
                    {
                        this.checkvalidation();
                        this.prodOverhead = (ProductionOverhead)s;
                        EditProductionOverhead editPoverhead = new EditProductionOverhead();
                        editPoverhead.DataContext = this;
                        editPoverhead.ShowDialog();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                    try
                    {
                        this.production = (Production)s;
                        this.prodOverhead.productionId = this.production.id;
                        AddProductionOverhead prodoverhead = new AddProductionOverhead();
                        prodoverhead.DataContext = this;
                        prodoverhead.ShowDialog();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                    try
                    {
                        EditProductionProduct epp = new EditProductionProduct();
                        this.productionProduct = (ProductionProduct)s;
                        this.checkvalidation();
                        epp.DataContext = this;
                        epp.ShowDialog();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public DelegateCommand<object> submitProduction
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        //check if production is approved
                        //move 

                        //update production as closed

                    });
                });
            }
        }

        private double _totalDougt;

        public double totalDougt
        {
            get { return _totalDougt; }
            set
            {
                _totalDougt = value;
                this.NotifyPropertyChanged("totalDougt");
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

                             Product pDuct = productDao.byId(this.productionProduct.productId);
                             double expectedW = 0.05 * pDuct.weight;
                             this.productionProduct.productionId = production.id;

                             if ((this.productionProduct.weight > (pDuct.weight + expectedW)) || (this.productionProduct.weight < (pDuct.weight - expectedW)))
                             {
                                 throw new Exception($"Product weight must not be greater than {Math.Round(pDuct.weight + expectedW, 2)} or less that {Math.Round(pDuct.weight - expectedW, 2)}");
                             }

                             double overheadSum = this.prodDao.byproductionId(this.productionProduct.productionId).Sum(x => x.overheadCount);

                             double totalIngredentCost = this.PIDao.sumtotalIngredientInKg(this.productionProduct.productionId);
                             double totalDough = this.pProductDao.byproductionId(this.productionProduct.productionId)
                             .Where(p => p.id != productionProduct.id).Sum(x => (x.weight * x.quantity));

                             totalDough = totalDough + (this.productionProduct.quantity * this.productionProduct.weight);

                             string query = "";
                             foreach (var item in this.productionProducts)
                             {
                                 if (item.productId == this.productionProduct.productId)
                                 {
                                     item.overheadCost = Math.Round(WeightAverageCostUtil.ProductOverheadUnitCost(this.productionProduct.weight, overheadSum, totalDough), 2);
                                     item.ingredientCost = Math.Round(WeightAverageCostUtil.ProductIngredentUnitCost(this.productionProduct.weight, totalIngredentCost, totalDough), 2);
                                     query = query + this.pProductDao.updateString(item, this.productionProduct.quantity, this.productionProduct.weight);
                                 }
                                 else
                                 {
                                     item.overheadCost = Math.Round(WeightAverageCostUtil.ProductOverheadUnitCost(item.weight, overheadSum, totalDough), 2);
                                     item.ingredientCost = Math.Round(WeightAverageCostUtil.ProductIngredentUnitCost(item.weight, totalIngredentCost, totalDough), 2);
                                     query = query + this.pProductDao.updateString(item);
                                 }
                             }

                             bool result = pProductDao.execute(query);// pProductDao.update(this.productionProduct);
                             if (result)
                             {
                                 this.isSpin = Visibility.Collapsed;
                                 MessageBox.Show("saved");
                                 List<ProductionProduct> lstP = pProductDao.byproductionId(this.productionProduct.productionId);
                                 this.productionProducts = new ObservableCollection<ProductionProduct>(lstP);
                                 this.mpp = new ObservableCollection<ProductionProduct>(lstP);
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
                                Product pDuct = productDao.byId(this.productionProduct.productId);
                                double expectedW = 0.05 * pDuct.weight;
                                this.productionProduct.productionId = production.id;

                                if ((this.productionProduct.weight > (pDuct.weight + expectedW)) || (this.productionProduct.weight < (pDuct.weight - expectedW)))
                                {
                                    throw new Exception($"Product weight must not be greater than {Math.Round(pDuct.weight + expectedW, 2)} or less that {Math.Round(pDuct.weight - expectedW, 2)}");
                                }

                                ProductionProduct pp = this.pProductDao.byProductionproductId(this.productionProduct.productionId,
                                    this.productionProduct.productId);

                                // check if the available gram is not more than the recipe total gram
                                if (pp == null)
                                {
                                    //var prodproduction = this.productDao.byId(this.productionProduct.productId);

                                    double overheadSum = this.prodDao.byproductionId(this.productionProduct.productionId).Sum(x => (x.overheadCount));
                                    double totalIngredentCost = this.PIDao.sumtotalIngredientInKg(this.productionProduct.productionId);
                                    double totalDough = this.pProductDao.byproductionId(this.productionProduct.productionId).Sum(x => (x.weight * x.quantity));

                                    totalDough = totalDough + (this.productionProduct.quantity * this.productionProduct.weight);
                                    this.productionProduct.overheadCost = Math.Round(WeightAverageCostUtil.ProductOverheadUnitCost(productionProduct.weight, overheadSum, totalDough), 2);
                                    this.productionProduct.ingredientCost = Math.Round(WeightAverageCostUtil.ProductIngredentUnitCost(productionProduct.weight, totalIngredentCost, totalDough), 2);

                                    string query = "";
                                    foreach (var item in this.productionProducts)
                                    {
                                        item.overheadCost = Math.Round(WeightAverageCostUtil.ProductOverheadUnitCost(item.weight, overheadSum, totalDough), 2);
                                        item.ingredientCost = Math.Round(WeightAverageCostUtil.ProductIngredentUnitCost(item.weight, totalIngredentCost, totalDough), 2);
                                        query = query + this.pProductDao.updateString(item);
                                    }

                                    query = query + this.pProductDao.insertString(this.productionProduct);
                                    bool result = this.pProductDao.execute(query);
                                    if (result)
                                    {
                                        MessageBox.Show("Product has been added successfully");
                                        this.productionProducts = new ObservableCollection<ProductionProduct>(pProductDao.byproductionId(this.productionProduct.productionId));
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Selected product already existn");
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
                    try
                    {
                        AddProductionProduct prod = new AddProductionProduct();
                        productionProduct = new ProductionProduct();
                        this.production = (Production)s;
                        prod.DataContext = this;
                        prod.ShowDialog();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                            this.checkvalidation();
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

        private string _totalProdIngredent;

        public string totalProdIngredent
        {
            get { return _totalProdIngredent; }
            set
            {
                _totalProdIngredent = value;
                this.NotifyPropertyChanged("totalProdIngredent");
            }
        }


        public DelegateCommand<object> loadIngredentCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        ProductionIngredentView pi = new ProductionIngredentView();
                        this.production = (Production)s;
                        this.productionName = $"Production title {this.production.title}";
                        this.productionIngredents = new ObservableCollection<ProductionIngredent>(PIDao.byProductionId(this.production.id));
                        pi.DataContext = this;
                        pi.ShowDialog();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                this.totalProdIngredent = $"Bulk Dough Weight {value.Sum(x => x.amount)}kg";
                this.totalProdCost = $"Total Ingredient Cost #{ string.Format(CultureInfo.InvariantCulture, "{0:N0}", Math.Round(value.Sum(x => (x.unitCost * x.amount)), 2))}";
                this.NotifyPropertyChanged("productionIngredents");
            }
        }

        private string _totalProdCost;

        public string totalProdCost
        {
            get { return _totalProdCost; }
            set
            {
                _totalProdCost = value;

                this.NotifyPropertyChanged("totalProdCost");
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
                                throw new Exception("Amount of ingredent left in stock is " + ru.quantity);
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
                            Production p = (Production)s;
                            this.production = p;
                            checkvalidation();
                            MessageBoxResult msg = MessageBox.Show("Are you sure", "Approve Production", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (msg == MessageBoxResult.No)
                            {
                                return;
                            }

                            PIDao.checkIngredentAvalabilityByProdId(p.id);

                            var ru = this.appConfigDao.read();
                            //bool result = PIDao.changeProdApprovalStatus(ProductionStatus.APPROVED.ToString(),
                            //    p.id, ru.username);

                            string query = PIDao.changeProdApprovalStatusQuery(ProductionStatus.APPROVED.ToString(),
                                p.id, ru.username);

                            List<ProductionIngredent> lstPI = PIDao.byProductionId(this.production.id);
                            //update ingredient weight
                            query = query + ingredentDao.updateIngredientQuantityQuery(this.production, lstPI);

                            bool result = PIDao.execute(query);
                            if (result)
                            {
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

        #region Manage Production Product

        private ObservableCollection<ProductionProduct> _mpp;

        public ObservableCollection<ProductionProduct> mpp
        {
            get
            {
                List<ProductionProduct> lst = pProductDao.byproductionId(this.production.id);
                _mpp = new ObservableCollection<ProductionProduct>(lst);
                var productWeight = pProductDao.sumTotalProductInKg(lst);
                totalProductWeight = productWeight.ToString();
                var recipeWeight = PIDao.byProductionId(this.production.id).Sum(x => x.amount);
                totalRecipeDough = recipeWeight.ToString();
                totalDoughDiff = $"{(productWeight > recipeWeight ? (productWeight - recipeWeight) : (recipeWeight - productWeight))}";
                return _mpp;
            }
            set
            {
                _mpp = value;
                this.NotifyPropertyChanged("mpp");
            }
        }

        private string _totalProductWeight;

        public string totalProductWeight
        {
            get { return _totalProductWeight; }
            set
            {
                _totalProductWeight = $"Product Dough Weight:  {value}kg";
                this.NotifyPropertyChanged("totalProductWeight");
            }
        }

        private string _totalRecipeDough;

        public string totalRecipeDough
        {
            get { return _totalRecipeDough; }
            set
            {
                _totalRecipeDough = $"Recipe Dough Weight:  {value}kg";
                this.NotifyPropertyChanged("totalRecipeDough");
            }
        }

        private string _totalDoughDiff;

        public string totalDoughDiff
        {
            get { return _totalDoughDiff; }
            set
            {
                _totalDoughDiff = $"Dough Weight Difference:  {value}kg";
                this.NotifyPropertyChanged("totalDoughDiff");
            }
        }




        public DelegateCommand<object> loadManageProduction
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        ManageProductionProduct mp = new ManageProductionProduct();
                        this.production = (Production)s;
                        mp.DataContext = this;
                        mp.ShowDialog();
                        this.production = new Production();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        private double _totalP;

        public double totalP
        {
            get
            {
                _totalP = PIDao.sumtotalIngredientInKg(this.production.id);
                return _totalP;
            }
            set
            {
                _totalP = value;
                this.NotifyPropertyChanged("totalP");
            }
        }

        private string _totalProduction;

        public string totalProduction
        {
            get
            {
                _totalProduction = $"Total Recipe Cost #{ string.Format(CultureInfo.InvariantCulture, "{0:N0}", Math.Round(this.totalP, 2))}";
                return _totalProduction;
            }
            set
            {
                _totalProduction = value;
                this.NotifyPropertyChanged("totalProduction");
            }
        }

        public DelegateCommand<object> loadProdIngredent
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        this.checkvalidation();
                        EditProductionRecipe ePRecipe = new EditProductionRecipe();
                        this.prodIngredient = (ProductionIngredent)s;
                        ePRecipe.DataContext = this;
                        ePRecipe.ShowDialog();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public DelegateCommand<object> updateProdIngredent
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
                            if (this.prodIngredient.amount < 0)
                            {
                                throw new Exception("Quantity inputted is invalid");
                            }
                            bool tr = PIDao.Update(this.prodIngredient);
                            if (tr)
                            {
                                this.productionIngredents = new ObservableCollection<ProductionIngredent>(PIDao.byProductionId(this.prodIngredient.productionId));
                                MessageBox.Show("Save", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                    this.isSpin = Visibility.Collapsed;
                });
            }
        }

        private ProductionIngredent _prodIngredient = new ProductionIngredent();

        public ProductionIngredent prodIngredient
        {
            get { return _prodIngredient; }
            set
            {
                _prodIngredient = value;
                this.NotifyPropertyChanged("prodIngredient");
            }
        }

        public ProductInventoryHistoryDao pihDao
        {
            get
            {
                return new ProductInventoryHistoryDao();
            }
        }

        public DelegateCommand<object> UpdateMoveToSalesCommand
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
                            // this.checkvalidation();
                            if (this.production.approval == ProductionStatus.CLOSED.ToString())
                            {
                                return;
                            }
                            else if (this.production.approval == ProductionStatus.NOT_APPROVED.ToString())
                            {
                                MessageBox.Show("Production need approval before movement to sales");
                                return;
                            }

                            // check product

                            if (this.productionProducts.Count < 1)
                            {
                                MessageBox.Show("Product produced have not been added");
                                return;
                            }

                            List<ProductionProduct> lst = pProductDao.byproductionId(this.production.id);
                            double ds = pProductDao.sumTotalProductIngram(lst);
                            if (totalP > ds)
                            {
                                MessageBoxResult msg = MessageBox.Show("Total dough weight is more than the weight of the product. Do you want to continue?",
                                    "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (msg == MessageBoxResult.No)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                MessageBoxResult msg = MessageBox.Show("Total dough weight is less than the weight of the product. Do you want to continue?",
                                    "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (msg == MessageBoxResult.No)
                                {
                                    return;
                                }
                            }

                            List<Product> lstProduct = productDao.all();
                            String query = "";
                            foreach (var tm in mpp)
                            {
                                ProductInventoryHistory pih = new ProductInventoryHistory();
                                pih.inventoryMode = InventoryModeEnum.ADD.ToString();
                                pih.createdDate = DateTime.Now;
                                pih.createdBy = appConfigDao.read().username;
                                pih.productId = tm.productId;
                                pih.quantity = tm.quantity;

                                query = query + pihDao.insertQuery(pih);
                                Product p = lstProduct.FirstOrDefault(x => x.id == tm.productId);

                                if (p != null)
                                {
                                    p.inventoryStore = String.IsNullOrEmpty(p.inventoryStore.ToString().Trim()) ? 0 : p.inventoryStore;
                                    query = query + productDao.updateStoreQuery(new Product() { id = tm.productId, inventoryStore = (p.inventoryStore + pih.quantity) });
                                }
                            }
                            this.production.approval = ProductionStatus.CLOSED.ToString();
                            query = query + dao.updateApprovalStatusQuery(this.production);

                            //List<ProductionIngredent> lstPI = PIDao.byProductionId(this.production.id);
                            ////update ingredient weight
                            //query = query + ingredentDao.updateIngredientQuantityQuery(this.production, lstPI);

                            bool result = pihDao.add(query);
                            if (result)
                            {
                                MessageBox.Show("Product have been move to sales", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });

                    this.isSpin = Visibility.Collapsed;
                });
            }
        }

        public void checkvalidation()
        {
            if (this.production.approval != null)
            {
                if (this.production.approval == ProductionStatus.APPROVED.ToString())
                {
                    throw new Exception("Approved production can't be edited or re-approve");
                }
                else if (this.production.approval == ProductionStatus.CLOSED.ToString())
                {
                    throw new Exception("Closed production can't be edited");
                }
            }
        }

        #endregion

    }
}
