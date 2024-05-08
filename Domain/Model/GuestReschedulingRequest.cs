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
        private int id {  get; set; }
        private int reservedAccommodationId { get; set; }
        private int accommodationId { get; set; }
        private int guestId { get; set; }
        private DateTime checkInDate { get; set; }
        private DateTime checkOutDate { get; set; }
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
        public int ReservedAccommodationId
        {
            get
            {
                return reservedAccommodationId;
            }
            set
            {
                if (value != reservedAccommodationId)
                {
                    reservedAccommodationId = value;
                    OnPropertyChanged(nameof(reservedAccommodationId));
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
                ReservedAccommodationId.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                CheckInDate.ToString(),
                CheckOutDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservedAccommodationId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            GuestId = Convert.ToInt32(values[3]);
            CheckInDate = Convert.ToDateTime(values[4]);
            CheckOutDate = Convert.ToDateTime(values[5]);
        }


        //owner - accepting or declining the request
        public string imagePath { get; set; }
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                if (value != imagePath)
                {
                    imagePath = value;
                    OnPropertyChanged(nameof(imagePath));
                }
            }
        }
    }
}
