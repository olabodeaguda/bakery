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
    public class SalesModelView : INotifyPropertyChanged
    {
        private string _searchHisory;

        public string searchHisory
        {
            get { return _searchHisory; }
            set
            {
                if (_searchHisory != value)
                {
                    _searchHisory = value;
                    List<CartModel> s = dailyHistory.Where(x => x.pName.ToLower().Contains(value) || x.customerName.ToLower().Contains(value)).ToList();
                    this.dailyCartHistory = new ObservableCollection<CartModel>(s);                    
                    this.NotifyPropertyChanged("searchHisory");
                }
            }
        }

        public List<CartModel> dailyHistory
        {
            get
            {
                return this.cartDao.byDaily(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private ObservableCollection<CartModel> _dailyCartHistory;

        public ObservableCollection<CartModel> dailyCartHistory
        {
            get
            {
                return _dailyCartHistory;
            }
            set
            {
                _dailyCartHistory = value;
                this.NotifyPropertyChanged("dailyCartHistory");
            }
        }


        public DelegateCommand<object> CheckOutCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (this.cartModel.itemLst.Count <= 0)
                            {
                                return;
                            }
                            MessageBoxResult mresult = MessageBox.Show("Are you sure?", "Submit Cart", MessageBoxButton.YesNo, MessageBoxImage.Information);

                            if(mresult == MessageBoxResult.No)
                            {
                                return;
                            }

                            _cartModel.cartStatus = CartStatus.PROCESSING.ToString();
                            List<CartModel> cartByDate = cartDao.byDate(DateTime.Now.ToString("yyyy-MM-dd"));
                            this.cartModel.customerName = $"Customer{cartByDate.Count + 1}";
                            int cartId = cartDao.add(this.cartModel);
                            string cardString = string.Empty;
                            List<Product> productsCurrent = lstItem;

                            foreach (CartProductModel tm in cartModel.itemLst)
                            {
                                Product p = productsCurrent.FirstOrDefault(x => x.id == tm.productId);

                                if (p == null)
                                {
                                    this.itemLst = new ObservableCollection<Product>(productsCurrent);
                                    throw new Exception($"{tm.productName} is no more available");
                                }
                                if (p.inventoryStore < tm.quantity)
                                {
                                    this.itemLst = new ObservableCollection<Product>(productsCurrent);
                                    throw new Exception($"{tm.productName} does not have up to the requested quantity in inventory.");
                                }

                                tm.cartId = cartId;

                                cardString = cardString + cartProductDao.getInsertQuery(tm);
                                cardString = cardString + productDao.updateStoreQuery(new Product()
                                {
                                    id = tm.productId,
                                    inventoryStore = p.inventoryStore - tm.quantity
                                });

                                //update Inventory history
                                cardString = cardString + PIHDao.insertQuery(new ProductInventoryHistory()
                                {
                                    createdBy = this.cartModel.createdBy,
                                    inventoryMode = InventoryMode.ADD.ToString(),
                                    productId = tm.productId,
                                    quantity = tm.quantity
                                });

                            }

                            cardString = cardString + cartDao.updateQuery(new CartModel() { id = cartId, cartStatus = CartStatus.DONE.ToString() });

                            bool d = cartDao.executeQuery(cardString);
                            if (d)
                            {
                                MessageBox.Show("Card was successfull", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    this.dailyCartHistory = new ObservableCollection<CartModel>(dailyHistory);
                                    this.cartModel.itemLst.Clear();
                                    itemLst = new ObservableCollection<Product>(this.lstItem);
                                });
                            }
                            else
                            {
                                throw new Exception("Transaction was not successfull please try again, or contact administrator");
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

        public ProductInventoryHistoryDao PIHDao
        {
            get
            {
                return new ProductInventoryHistoryDao();
            }
        }

        public ProductDao productDao
        {
            get
            {
                return new ProductDao();
            }
        }

        public AppConfigDao appConfigDao
        {
            get
            {
                return new AppConfigDao();
            }
        }

        public CartDao cartDao
        {
            get
            {
                return new CartDao();
            }
        }

        public CartProductDao cartProductDao
        {
            get
            {
                return new CartProductDao();
            }
        }

        public DelegateCommand<object> enterKeyCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    if (this.cartModel.itemLst.Count < 1)
                    {
                        return;
                    }

                    var sd = this.cartModel;
                    cartSummary summary = new cartSummary();
                    summary.DataContext = this;
                    bool? close = summary.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> clrCartCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.cartModel.itemLst.Clear();
                    });
                });
            }
        }


        private string _searchItem;

        public string searchItem
        {
            get { return _searchItem; }
            set
            {
                if (_searchItem != value)
                {
                    _searchItem = value;
                    itemLst = new ObservableCollection<Product>(this.lstItem.Where(x => x.name.ToLower().Contains(value.ToLower())));
                    this.NotifyPropertyChanged("searchItem");
                }
            }
        }

        public LoginModel loginModel
        {
            get
            {
                return appConfigDao.read();
            }
        }

        private CartModel _cartModel = new CartModel();

        public CartModel cartModel
        {
            get
            {
                _cartModel.createdBy = this.loginModel.fullname;
                return _cartModel;
            }
            set
            {
                _cartModel = value;
                this.NotifyPropertyChanged("cartModel");
            }
        }


        private List<Product> lstItem
        {
            get
            {
                return prodDao.all();
            }
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        itemLst = new ObservableCollection<Product>(this.lstItem);
                        dailyCartHistory = new ObservableCollection<CartModel>(this.dailyHistory);
                    });
                });
            }
        }

        private Product _selectedProduct = new Product();

        public Product selectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                this.NotifyPropertyChanged("selectedProduct");
            }
        }


        public DelegateCommand<object> itmLstDblCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        if (s != null)
                        {
                            Product p = this.selectedProduct;
                            CartProductModel newCpm = new CartProductModel()
                            {
                                price = cartModel.isRetails ? p.retailPrice : p.wholeSales,
                                productId = p.id,
                                productName = p.name,
                                quantity = 1,
                                isRetail = cartModel.isRetails
                            };
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                CartProductModel cpm = cartModel.itemLst.FirstOrDefault(x => x.productId == newCpm.productId);
                                if (cpm != null)
                                {
                                    int indx = cartModel.itemLst.IndexOf(cpm);
                                    cartModel.itemLst.Remove(cpm);
                                    cpm.quantity = cpm.quantity + newCpm.quantity;
                                    cpm.price = newCpm.price * cpm.quantity;
                                    cartModel.itemLst.Insert(indx, cpm);
                                }
                                else
                                {
                                    cartModel.itemLst.Add(newCpm);
                                }

                            });
                        }
                    });
                });
            }
        }

        public ProductDao prodDao
        {
            get
            {
                return new ProductDao();
            }
        }

        private ObservableCollection<Product> _cartLst;

        public ObservableCollection<Product> cartLst
        {
            get
            {
                return _cartLst;
            }
            set
            {
                _cartLst = value;
                this.NotifyPropertyChanged("cartLst");
            }
        }

        private ObservableCollection<Product> _itemLst;

        public ObservableCollection<Product> itemLst
        {
            get
            {
                return _itemLst;
            }
            set
            {
                _itemLst = value;
                this.NotifyPropertyChanged("itemLst");
            }
        }

        public double winHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight;
            }
        }

        public double winWidth
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth;
            }
        }

        #region property change

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
