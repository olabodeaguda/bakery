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
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                this.grpoverheadLst = new ObservableCollection<OverheadDetails>(overheadDetailsDao.allSingle());
                            });

                        }
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
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (overheaddetailGrp.id == -1)
                            {
                                return;
                            }
                            else if (overheaddetailGrp.overheadId == -1)
                            {
                                throw new Exception("overhead is required");
                            }
                            else if (overheaddetailGrp.quantity < 1)
                            {
                                throw new Exception("Quantity is required");
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


        public DelegateCommand<object> loadOverheadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddOverheadGrp addoverhead = new AddOverheadGrp();
                    addoverhead.DataContext = this;
                    addoverhead.ShowDialog();
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
                            if (String.IsNullOrEmpty(overheadDetail.groupName))
                            {
                                return;
                            }

                            int id = overheadDetailsDao.add(this.overheadDetail);
                            if (id > 0)
                            {
                                this.isAddoverhead = true;
                                this.overheadDetail.id = id;
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Update", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    });
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

        public List<OverheadDetails> GrpLst
        {
            get
            {
                return overheadDetailsDao.all();
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
