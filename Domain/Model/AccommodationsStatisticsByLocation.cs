using BookingApp.View.Guest.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class AccommodationsStatisticsByLocation : INotifyPropertyChanged
    {
        private int locationId {  get; set; }
        private List<Accommodation> accommodations { get; set; }
        private int reservations {  get; set; }
        private double busyness {  get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public AccommodationsStatisticsByLocation()
        {
            accommodations = new List<Accommodation>();
            reservations = 0;
        }
        public int LocationId
        {
            get
            {
                return locationId;
            }
            set
            {
                if (value != locationId)
                {
                    locationId = value;
                    OnPropertyChanged(nameof(locationId));
                }
            }
        }
        public int Reservations
        {
            get
            {
                return reservations;
            }
            set
            {
                if (value != reservations)
                {
                    reservations = value;
                    OnPropertyChanged(nameof(reservations));
                }
            }
        }
        public double Busyness
        {
            get
            {
                return busyness;
            }
            set
            {
                if (value != busyness)
                {
                    busyness = value;
                    OnPropertyChanged(nameof(busyness));
                }
            }
        }
        public List<Accommodation> Accommodations
        {
            get
            {
                return accommodations;
            }
            set
            {
                if (value != accommodations)
                {
                    accommodations = value;
                    OnPropertyChanged(nameof(accommodations));
                }
            }
        }
    }
}
