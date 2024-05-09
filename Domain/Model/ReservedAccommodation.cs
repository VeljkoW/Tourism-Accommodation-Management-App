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
using BookingApp.Services;

namespace BookingApp.Domain.Model
{
    public class ReservedAccommodation : ISerializable, INotifyPropertyChanged
    {
        private int id { get; set; }
        private int guestId { get; set; }
        private DateTime checkInDate { get; set; }
        private DateTime checkOutDate { get; set; }
        private Accommodation accommodation { get; set; }

        public ReservedAccommodation() 
        {
            accommodation = new Accommodation();
        }
        public ReservedAccommodation(int guestId, int accommodationId, DateTime checkInDate, DateTime checkOutDate)
        {
            this.guestId = guestId;
            this.checkInDate = checkInDate;
            this.checkOutDate = checkOutDate;
            accommodation = new Accommodation();
            accommodation = AccommodationService.GetInstance().GetById(accommodationId);
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
        public Accommodation Accommodation
        {
            get
            {
                return accommodation;
            }
            set
            {
                if (value != accommodation)
                {
                    accommodation = value;
                    OnPropertyChanged(nameof(accommodation));
                }
            }
        }


        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                Accommodation.Id.ToString(),
                GuestId.ToString(),
                CheckInDate.ToString(),
                CheckOutDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            int accommodationId = Convert.ToInt32(values[1]);
            Accommodation = AccommodationService.GetInstance().GetById(accommodationId);
            GuestId = Convert.ToInt32(values[2]);
            CheckInDate = Convert.ToDateTime(values[3]);
            CheckOutDate = Convert.ToDateTime(values[4]);
        }
        public string Print
        {
            get
            {
                UserRepository userRepository = new UserRepository();
                User user = new User();
                user = userRepository.GetById(GuestId);
                return "Remaining " + (5 - (DateTime.Now - CheckOutDate).Days) + " days to rate the user: " + user.Username;
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
