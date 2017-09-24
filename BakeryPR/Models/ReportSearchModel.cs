using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class ReportSearchModel : INotifyPropertyChanged
    {
        private DateTime _startDate = DateTime.Now;

        public DateTime startDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                this.NotifyPropertyChanged("startDate");
            }
        }

        private string _startDateDisplay = DateTime.Now.ToShortDateString();

        public string startDateDisplay
        {
            get { return _startDateDisplay; }
            set
            {
                _startDateDisplay = value;
                this.NotifyPropertyChanged("startDateDisplay");
            }
        }

        private DateTime _endDate = DateTime.Now;

        public DateTime endDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                this.NotifyPropertyChanged("endDate");
            }
        }

        private string _endDateDisplay = DateTime.Now.ToShortDateString();

        public string endDateDisplay
        {
            get { return _endDateDisplay; }
            set
            {
                _endDateDisplay = value;
                this.NotifyPropertyChanged("endDateDisplay");
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
