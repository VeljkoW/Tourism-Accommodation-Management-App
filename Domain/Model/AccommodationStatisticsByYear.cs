using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class AccommodationStatisticsByYear : INotifyPropertyChanged
    {
        private int accommodationId;
        private int year;
        private int reservations;
        private int cancellations;
        private int reschedulings;
        private int recommendedRenovations; 
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public AccommodationStatisticsByYear()
        {
            reservations = 0; cancellations = 0; reschedulings = 0; recommendedRenovations = 0;
        }
        public AccommodationStatisticsByYear(AccommodationStatisticsByYear accommodationStatisticsByYear)
        {
            accommodationId = accommodationStatisticsByYear.accommodationId;
            year = accommodationStatisticsByYear.year;
            reservations = accommodationStatisticsByYear.reservations;
            cancellations = accommodationStatisticsByYear.cancellations;
            reschedulings = accommodationStatisticsByYear.reschedulings;
            recommendedRenovations = accommodationStatisticsByYear.recommendedRenovations;
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
        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                if (value != year)
                {
                    year = value;
                    OnPropertyChanged(nameof(year));
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
        public int Cancellations
        {
            get
            {
                return cancellations;
            }
            set
            {
                if (value != cancellations)
                {
                    cancellations = value;
                    OnPropertyChanged(nameof(cancellations));
                }
            }
        }
        public int Reschedulings
        {
            get
            {
                return reschedulings;
            }
            set
            {
                if (value != reschedulings)
                {
                    reschedulings = value;
                    OnPropertyChanged(nameof(reschedulings));
                }
            }
        }
        public int RecommendedRenovations
        {
            get
            {
                return recommendedRenovations;
            }
            set
            {
                if (value != recommendedRenovations)
                {
                    recommendedRenovations = value;
                    OnPropertyChanged(nameof(recommendedRenovations));
                }
            }
        }
    }
}
