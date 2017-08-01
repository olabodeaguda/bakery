using BakeryPR.DAO;
using BakeryPR.Models;
using BakeryPR.Utilities;
using BakeryPR.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BakeryPR.ModelView
{
    public class GrpOverheadModelView : INotifyPropertyChanged
    {
        private OverheadDetailsGroup _overheaddetailGrp;

        public OverheadDetailsGroup overheaddetailGrp
        {
            get { return _overheaddetailGrp; }
            set
            {
                _overheaddetailGrp = value;
                this.NotifyPropertyChanged("overheaddetailGrp");
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

        public DelegateCommand<object> addOverheadCommand
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
