using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class AccommodationForReservation : INotifyPropertyChanged
    {
        private List<AvailableDate> availableDates { get; set; }
        private int accommodationId {  get; set; }
        private int guestNumber {  get; set; }
        public AccommodationForReservation() 
        {
            availableDates = new List<AvailableDate>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public int AccommodationId
        {
            get
            {
                return accommodationId;
            }
            set
            {
                if (value != accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged(nameof(accommodationId));
                }
            }
        }

        public int GuestNumber
        {
            get
            {
                return guestNumber;
            }
            set
            {
                if (value != guestNumber)
                {
                    guestNumber = value;
                    OnPropertyChanged(nameof(guestNumber));
                }
            }
        }
        public List<AvailableDate> AvailableDates
        {
            get
            {
                return availableDates;
            }
            set
            {
                if (value != availableDates)
                {
                    availableDates = value;
                    OnPropertyChanged(nameof(availableDates));
                }
            }
        }
    }
}
