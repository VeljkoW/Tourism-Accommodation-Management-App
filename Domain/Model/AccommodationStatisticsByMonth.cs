using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class AccommodationStatisticsByMonth : INotifyPropertyChanged
    {
        private int accommodationId;
        private int month;
        private string monthString;
        private int reservations;
        private int cancellations;
        private int reschedulings;
        private int recommendedRenovations; public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public AccommodationStatisticsByMonth(int month)
        {
            this.month = month;
            MonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
            reservations = 0; cancellations = 0; reschedulings = 0; recommendedRenovations = 0;
        }
        public AccommodationStatisticsByMonth(AccommodationStatisticsByMonth accommodationStatisticsByMonth)
        {
            accommodationId = accommodationStatisticsByMonth.accommodationId;
            month = accommodationStatisticsByMonth.month;
            reservations = accommodationStatisticsByMonth.reservations;
            cancellations = accommodationStatisticsByMonth.cancellations;
            reschedulings = accommodationStatisticsByMonth.reschedulings;
            recommendedRenovations = accommodationStatisticsByMonth.recommendedRenovations;
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
        public int Month
        {
            get
            {
                return month;
            }
            set
            {
                if (value != month)
                {
                    month = value;
                    OnPropertyChanged(nameof(month));
                }
            }
        }
        public string MonthString
        {
            get
            {
                return monthString;
            }
            set
            {
                if (value != monthString)
                {
                    monthString = value;
                    OnPropertyChanged(nameof(monthString));
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
