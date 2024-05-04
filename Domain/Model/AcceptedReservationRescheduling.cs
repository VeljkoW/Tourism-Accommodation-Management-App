using System;
using System.Collections.Generic;
using BookingApp.Serializer;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class AcceptedReservationRescheduling : INotifyPropertyChanged, ISerializable
    {

        public int accommodationId { get; set; }

        public int guestId { get; set; }

        public DateTime acceptedDate { get; set; }

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
        public int GuestId
        {
            get
            {
                return guestId;
            }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged(nameof(guestId));
                }
            }
        }

        public DateTime AcceptedDate
        {
            get
            {
                return acceptedDate;
            }
            set
            {
                if (value != acceptedDate)
                {
                    acceptedDate = value;
                    OnPropertyChanged(nameof(acceptedDate));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                AccommodationId.ToString(),
                GuestId.ToString(),
                AcceptedDate.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            AccommodationId = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            AcceptedDate = Convert.ToDateTime(values[2]);
        }

    }
}
