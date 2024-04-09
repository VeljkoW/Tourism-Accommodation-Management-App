using System;
using System.Collections.Generic;
using System.ComponentModel;
using BookingApp.Serializer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using BookingApp.Repository;

namespace BookingApp.Domain.Model
{
    public class ReservedAccommodation : ISerializable, INotifyPropertyChanged
    {
        public int guestId { get; set; }
        public int accommodationId { get; set; }
        public DateTime checkInDate { get; set; }
        public DateTime checkOutDate { get; set; }

        public ReservedAccommodation() { }

        public ReservedAccommodation(int guestId, int accommodationId, DateTime checkInDate, DateTime checkOutDate)
        {
            this.guestId = guestId;
            this.accommodationId = accommodationId;
            this.checkInDate = checkInDate;
            this.checkOutDate = checkOutDate;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
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


        public string[] ToCSV()
        {
            string[] csvValues = {
                accommodationId.ToString(),
                guestId.ToString(),
                checkInDate.ToString(),
                checkOutDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            accommodationId = Convert.ToInt32(values[0]);
            guestId = Convert.ToInt32(values[1]);
            checkInDate = Convert.ToDateTime(values[2]);
            checkOutDate = Convert.ToDateTime(values[3]);
        }
        public string Print
        {
            get
            {
                UserRepository userRepository = new UserRepository();
                User user = new User();
                user = userRepository.GetById(guestId);
                return "Remaining " + (5 - (DateTime.Now - checkOutDate).Days) + " days to rate the user: " + user.Username;
            }
            set
            {
                if (value != Print)
                {
                    Print = value;
                    OnPropertyChanged("Print");
                }
            }
        }
    }
}
