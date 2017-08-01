using BakeryPR.DAO;
using BakeryPR.Models;
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
        public OverheadDetailsDao overheadDao
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


        public List<OverheadDetails> GrpLst
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
