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
    public class OverheadModelView : INotifyPropertyChanged
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

        public DelegateCommand<object> loadCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    AddOverhead prod = new AddOverhead();
                    prod.DataContext = this;
                    prod.ShowDialog();
                    this.overheads = new ObservableCollection<Overhead>(dao.all());
                });
            }
        }

        public DelegateCommand<object> UpdateCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    try
                    {
                        Overhead sd = this.overhead;
                        sd.mTypeId = 50;
                        bool result = dao.update(sd);
                        if (result)
                        {
                            MessageBox.Show("Update was successfull");
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
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
                        Overhead ig = this.overhead;
                        ig.mTypeId = 50;

                        var isExist = dao.all().FirstOrDefault(x => x.name.ToLower() == ig.name.ToLower());
                        if (isExist != null)
                        {
                            MessageBox.Show($"{ig.name} already exists", "Duplicate", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }                        

                        bool result = dao.add(ig);
                        if (result)
                        {
                            MessageBox.Show("Saved");
                        }
                        else
                        {
                            MessageBox.Show("not saved");
                        }
                        this.overhead = new Overhead();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

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
                        EditOverhead editIngr = new EditOverhead();
                        this.overhead = (Overhead)s;

                        editIngr.DataContext = this;
                        bool? result = editIngr.ShowDialog();
                        this.overheads = new ObservableCollection<Overhead>(dao.all());
                        this.overhead = new Overhead(); 
                    }
                });
            }
        }

        public DelegateCommand<object> loadDeleteCommand
        {
            get
            {
                return new DelegateCommand<object>((s) =>
                {
                    Overhead p = (Overhead)s;
                    if (p == null)
                    {
                        return;
                    }
                    MessageBoxResult msg = MessageBox.Show("Are you sure you want to delete this overhead ?", "Deletion", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (msg == MessageBoxResult.Yes)
                    {
                        bool result = dao.Delete(p.id);
                        if (result)
                        {
                            this.overheads = new ObservableCollection<Overhead>(dao.all());
                            MessageBox.Show($"{p.name} has been deteled successfully");
                        }
                        else
                        {
                            MessageBox.Show($"{p.name} has not been deteled. Please try again or contat administrator");
                        }
                    }
                });
            }
        }

        public OverheadDao dao
        {
            get
            {
                return new OverheadDao();
            }
        }

        private MeasureTypeDao mDao
        {
            get
            {
                return new MeasureTypeDao();
            }
        }

        private Overhead _overhead = new Overhead();

        public Overhead overhead
        {
            get { return _overhead; }
            set
            {
                _overhead = value;
                this.NotifyPropertyChanged("overhead");
            }
        }

        private ObservableCollection<Overhead> _overheads = new ObservableCollection<Overhead>();
        public ObservableCollection<Overhead> overheads
        {
            get
            {
                _overheads = new ObservableCollection<Overhead>(dao.all());
                return _overheads;
            }
            set
            {
                _overheads = value;
                this.NotifyPropertyChanged("overheads");
            }
        }

        public ObservableCollection<string> OverheadTypes
        {
            get
            {
                List<string> lst = new List<string>();
                lst.Add("none");
                lst.AddRange(Enum.GetNames(typeof(OverheadType)).ToList());
                return new ObservableCollection<string>(lst);
            }
        }

        public ObservableCollection<MeasurementType> MeasurementTypes
        {
            get
            {
                List<MeasurementType> lst = new List<MeasurementType>();
                lst.Add(new MeasurementType() { id = -1, name = "none" });
                lst.AddRange(mDao.all());
                return new ObservableCollection<MeasurementType>(lst);
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
