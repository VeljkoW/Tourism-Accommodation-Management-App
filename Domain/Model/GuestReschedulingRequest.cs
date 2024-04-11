using BookingApp.Serializer;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class GuestReschedulingRequest : ISerializable, INotifyPropertyChanged
    {
        public int id {  get; set; }
        public int accommodationId { get; set; }
        public int guestId { get; set; }

        public DateTime checkInDate { get; set; }

        public DateTime checkOutDate { get; set; }

        public GuestReschedulingRequest() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(nameof(id));
                }
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
        public DateTime CheckInDate
        {
            get
            {
                return checkInDate;
            }
            set
            {
                if (value != checkInDate)
                {
                    checkInDate = value;
                    OnPropertyChanged(nameof(checkInDate));
                }
            }
        }
        public DateTime CheckOutDate
        {
            get
            {
                return checkOutDate;
            }
            set
            {
                if (value != checkOutDate)
                {
                    checkOutDate = value;
                    OnPropertyChanged(nameof(checkOutDate));
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                accommodationId.ToString(),
                guestId.ToString(),
                checkInDate.ToString(),
                checkOutDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            accommodationId = Convert.ToInt32(values[1]);
            guestId = Convert.ToInt32(values[2]);
            checkInDate = Convert.ToDateTime(values[3]);
            checkOutDate = Convert.ToDateTime(values[4]);
        }

    }
}
