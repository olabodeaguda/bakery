using BakeryPR.DAO;
using BakeryPR.Models;
using BakeryPR.Utilities;
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

        public DelegateCommand<object> enterKeyCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    var sd = this.cartModel;
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

        private CartModel _cartModel = new CartModel();

        public CartModel cartModel
        {
            get { return _cartModel; }
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
