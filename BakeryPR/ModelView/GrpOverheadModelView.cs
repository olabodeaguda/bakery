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
    public class GrpOverheadModelView : INotifyPropertyChanged
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
        private OverheadDetailsGroup _overheaddetailGrp = new OverheadDetailsGroup();

        public OverheadDetailsGroup overheaddetailGrp
        {
            get { return _overheaddetailGrp; }
            set
            {
                _overheaddetailGrp = value;
                this.NotifyPropertyChanged("overheaddetailGrp");
            }
        }

        private ObservableCollection<OverheadDetails> _grpoverheadLst = new ObservableCollection<OverheadDetails>();

        public ObservableCollection<OverheadDetails> grpoverheadLst
        {
            get
            {
                _grpoverheadLst = new ObservableCollection<OverheadDetails>(overheadDetailsDao.allSingle());
                return _grpoverheadLst;
            }
            set
            {
                _grpoverheadLst = value;
                this.NotifyPropertyChanged("grpoverheadLst");
            }

        }

        public DelegateCommand<object> loadOvHeadCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        this.grpoverheadLst = new ObservableCollection<OverheadDetails>(overheadDetailsDao.allSingle());
                    });
                });
            }
        }

        public DelegateCommand<object> loadManageoverheadGrp
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {

                    overheadDetail = (OverheadDetails)s;
                    overHeaddetailsLst = new ObservableCollection<OverheadDetailsGroup>(overheadDetailGDao.byGrpId(overheadDetail.id));
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.NotifyPropertyChanged("overHeaddetailsLst"); //= new ObservableCollection<OverheadDetails>(overheadDetailsDao.allSingle());
                    });
                    ManageOverheadGrp mog = new ManageOverheadGrp();
                    mog.DataContext = this;
                    mog.ShowDialog();
                });
            }
        }

        private OverheadDetails _overheadDetail = new OverheadDetails();

        public OverheadDetails overheadDetail
        {
            get { return _overheadDetail; }
            set
            {
                _overheadDetail = value;
                this.NotifyPropertyChanged("overheadDetail");
            }
        }

        private bool _isAddoverhead = false;

        public bool isAddoverhead
        {
            get { return _isAddoverhead; }
            set
            {
                _isAddoverhead = value;
                this.NotifyPropertyChanged("isAddoverhead");
            }
        }

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    addGrpOverhead addgrp = new addGrpOverhead();
                    overheadDetail = new OverheadDetails();
                    addgrp.DataContext = this;
                    addgrp.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> loadUpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    overheadDetail = (OverheadDetails)s;
                    EditOverhead eov = new EditOverhead();
                    eov.DataContext = this;
                    eov.ShowDialog();

                });
            }
        }

        public DelegateCommand<object> loadOverheadUpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    overheadDetail = (OverheadDetails)s;
                    EditGrpOverhead eov = new EditGrpOverhead();
                    eov.DataContext = this;
                    eov.ShowDialog();

                });
            }
        }

        public DelegateCommand<object> addOverheadUpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    if (!string.IsNullOrEmpty(overheadDetail.groupName))
                    {
                        bool res = overheadDetailsDao.update(overheadDetail);
                        if (res)
                        {
                            MessageBox.Show("Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.grpoverheadLst = new ObservableCollection<OverheadDetails>(overheadDetailsDao.allSingle());
                        }
                    }
                });
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

        public OverheadDetailsGroupDao overheadDetailGDao
        {
            get
            {
                return new OverheadDetailsGroupDao();
            }
        }


        public DelegateCommand<object> addOverheadCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        this.isSpin = Visibility.Visible;
                        try
                        {
                            if (overheaddetailGrp.overheadId == -1)
                            {
                                throw new Exception("overhead is required");
                            }

                            else if (overheaddetailGrp.quantity < 1)
                            {
                                throw new Exception("Quantity is required");
                            }
                            overheaddetailGrp.grpId = overheadDetail.id;
                            bool res = overheadDetailGDao.add(overheaddetailGrp);
                            if (res)
                            {
                                overHeaddetailsLst = new ObservableCollection<OverheadDetailsGroup>(overheadDetailGDao.byGrpId(overheaddetailGrp.grpId));
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    this.NotifyPropertyChanged("overHeaddetailsLst");
                                });

                                MessageBox.Show("Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                throw new Exception("Not saved. Try again or contact administrator, if issue continue");
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        this.isSpin = Visibility.Collapsed;
                    });
                });
            }
        }

        public DelegateCommand<object> editOverheadGrp
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    overheaddetailGrp = (OverheadDetailsGroup)s;
                    EditOverheadGrp eog = new EditOverheadGrp();
                    eog.DataContext = this;
                    eog.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> editOverheadGrpUpdate
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    overheaddetailGrp = (OverheadDetailsGroup)s;
                    overheaddetailGrp.grpId = overheadDetail.id;
                    EditOverheadGrp eog = new EditOverheadGrp();
                    eog.DataContext = this;
                    eog.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> loadOverheadEditCommand
        {
            get
            {
                return new DelegateCommand<object>(async (s) =>
                {
                    await Task.Run(() =>
                    {
                        isSpin = Visibility.Visible;
                        try
                        {
                            bool t = overheadDetailGDao.update(this.overheaddetailGrp);
                            if (t)
                            {
                                overHeaddetailsLst = new ObservableCollection<OverheadDetailsGroup>(overheadDetailGDao.byGrpId(overheadDetail.id));
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    this.NotifyPropertyChanged("overHeaddetailsLst");
                                });
                                MessageBox.Show("Saved", "successfull", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public DelegateCommand<object> loadEditCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {

                });
            }
        }

        public DelegateCommand<object> loadOverheadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddOverheadGrp addoverhead = new AddOverheadGrp();
                    overheaddetailGrp = new OverheadDetailsGroup();
                    addoverhead.DataContext = this;
                    addoverhead.ShowDialog();
                });
            }
        }

        public DelegateCommand<object> addCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    //await Task.Run(() =>
                    //{
                    try
                    {
                        if (String.IsNullOrEmpty(overheadDetail.groupName))
                        {
                            return;
                        }

                        int id = overheadDetailsDao.add(this.overheadDetail);
                        if (id > 0)
                        {
                            this.isAddoverhead = true;
                            this.overheadDetail.id = id;
                            //addOverheadCommand
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                //overHeaddetailsLst
                                this.NotifyPropertyChanged("grpoverheadLst"); //= new ObservableCollection<OverheadDetails>(overheadDetailsDao.allSingle());
                            });

                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Update", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //});
                });
            }
        }

        public OverheadDetailsDao overheadDetailsDao
        {
            get
            {
                return new OverheadDetailsDao();
            }
        }

        private int _selectedGrp;

        public int selectedGrp
        {
            get { return _selectedGrp; }
            set
            {
                _selectedGrp = value;
                this.NotifyPropertyChanged("selectedGrp");
            }
        }

        public OverheadDao overheadDao
        {
            get
            {
                return new OverheadDao();
            }
        }

        public List<OverheadDetailsGroup> GrpLst
        {
            get
            {
                return overheadDetailGDao.all();
            }
        }

        public DelegateCommand<object> loadoverdetailslstCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.overHeaddetailsLst = new ObservableCollection<OverheadDetailsGroup>(overheadDetailGDao.byGrpId(this.overheadDetail.id));
                    });
                });
            }
        }

        private ObservableCollection<OverheadDetailsGroup> _overHeaddetailsLst = new ObservableCollection<OverheadDetailsGroup>();

        public ObservableCollection<OverheadDetailsGroup> overHeaddetailsLst
        {
            get { return _overHeaddetailsLst; }
            set
            {
                _overHeaddetailsLst = value;
                this.NotifyPropertyChanged("overHeaddetailsLst");
            }
        }


        public List<Overhead> overheadLst
        {
            get
            {
                return overheadDao.all();
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
