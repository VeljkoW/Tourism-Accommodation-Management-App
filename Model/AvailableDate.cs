using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class AvailableDate : INotifyPropertyChanged
    {
        public DateTime checkInDate { get; set; }

        public DateTime checkOutDate { get; set; }

        public AvailableDate() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public string PrintAvailableDate
        {
            get
            {
                return checkInDate + "  -  " + checkOutDate;
            }
            set
            {
                if (value != PrintAvailableDate)
                {
                    PrintAvailableDate = value;
                    OnPropertyChanged("PrintAvailableDate");
                }
            }
        }
    }
}

