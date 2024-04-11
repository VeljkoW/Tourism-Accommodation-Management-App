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
        public int id { get; set; } 
        public int guestId { get; set; }
        public int accommodationId { get; set; }
        public DateTime checkInDate { get; set; }
        public DateTime checkOutDate { get; set; }

        public string accommodationName { get; set; }

        public Location location { get; set; }

        public Image image { get; set; }
        
        public AccommodationType accommodationType { get; set; }

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
        public string AccommodationName
        {
            get
            {
                return accommodationName;
            }
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged(nameof(accommodationName));
                }
            }
        }
        public AccommodationType AccommodationType
        {
            get
            {
                return accommodationType;
            }
            set
            {
                if (value != accommodationType)
                {
                    accommodationType = value;
                    OnPropertyChanged(nameof(accommodationType));
                }
            }
        }
        public Location Location
        {
            get
            {
                return location;
            }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged(nameof(location));
                }
            }
        }
        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                if (value != image)
                {
                    image = value;
                    OnPropertyChanged(nameof(image));
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
            Accommodation accommodation = new Accommodation();
            accommodation = AccommodationService.GetInstance().GetById(accommodationId);
            accommodationName = accommodation.Name;
            location = accommodation.location;
            image = accommodation.Images[0];
            accommodationType = accommodation.AccommodationType;
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
