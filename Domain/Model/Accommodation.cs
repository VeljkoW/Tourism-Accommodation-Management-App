using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public enum AccommodationType { Apartment, House, Hut }
namespace BookingApp.Domain.Model
{
    public class Accommodation : ISerializable, INotifyPropertyChanged
    {
        public int id { get; set; }
        public int ownerId { get; set; }
        public string name { get; set; }
        public Location? location { get; set; }
        public AccommodationType accommodationType { get; set; }
        public int maxGuestNumber { get; set; }
        public int minReservationDays { get; set; }
        public int cancelationDaysLimit { get; set; }
        public List<Image> images { get; set; }

        public string recommended { get; set; }
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
        public string Recommended
        {
            get
            {
                return recommended;
            }
            set
            {
                if (value != recommended)
                {
                    recommended = value;
                    OnPropertyChanged(nameof(recommended));
                }
            }
        }
        public int OwnerId
        {
            get
            {
                return ownerId;
            }
            set
            {
                if (value != ownerId)
                {
                    ownerId = value;
                    OnPropertyChanged(nameof(ownerId));
                }
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged(nameof(name));
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

        public int MaxGuestNumber
        {
            get
            {
                return maxGuestNumber;
            }
            set
            {
                if (value != maxGuestNumber)
                {
                    maxGuestNumber = value;
                    OnPropertyChanged(nameof(maxGuestNumber));
                }
            }
        }

        public int MinReservationDays
        {
            get
            {
                return minReservationDays;
            }
            set
            {
                if (value != minReservationDays)
                {
                    minReservationDays = value;
                    OnPropertyChanged(nameof(minReservationDays));
                }
            }
        }

        public int CancelationDaysLimit
        {
            get
            {
                return cancelationDaysLimit;
            }
            set
            {
                if (value != cancelationDaysLimit)
                {
                    cancelationDaysLimit = value;
                    OnPropertyChanged(nameof(cancelationDaysLimit));
                }
            }
        }


        public List<Image> Images
        {
            get
            {
                return images;
            }
            set
            {
                if (value != images)
                {
                    images = value;
                    OnPropertyChanged(nameof(images));
                }
            }
        }
        public Accommodation()
        {
            Name = string.Empty;
            Location = new Location();
            AccommodationType = AccommodationType.Apartment;
            MaxGuestNumber = 0;
            MinReservationDays = 0;
            CancelationDaysLimit = 0;
            Images = new List<Image>();
        }
        public Accommodation(int ownerId, string Name, Location Location, AccommodationType AccommodationType,
                            int MaxGuestNumber, int MinReservationDays, int CancelationDaysLimit, List<Image> Images)
        {
            this.ownerId = ownerId;
            this.Name = Name;
            this.Location = Location;
            this.AccommodationType = AccommodationType;
            this.MaxGuestNumber = MaxGuestNumber;
            this.MinReservationDays = MinReservationDays;
            this.CancelationDaysLimit = CancelationDaysLimit;
            this.Images = Images;
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                OwnerId.ToString(),
                Name,
                Location.Id.ToString(),
                AccommodationType.ToString(),
                MaxGuestNumber.ToString(),
                MinReservationDays.ToString(),
                CancelationDaysLimit.ToString(),
                ImagesIdToCSV()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Name = values[2];
            LocationRepository LocationRepository = new LocationRepository();
            Location = LocationRepository.GetById(Convert.ToInt32(values[3]));
            AccommodationType = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[4]);
            MaxGuestNumber = Convert.ToInt32(values[5]);
            MinReservationDays = Convert.ToInt32(values[6]);
            CancelationDaysLimit = Convert.ToInt32(values[7]);
            if (values[8].Length > 0)
            {
                string[] ImageIds = values[8].Split(',');
                for (int i = 0; i < ImageIds.Length; i++)
                {
                    Image image = new Image();
                    ImageRepository imageRepository = new ImageRepository();
                    image = imageRepository.GetById(Convert.ToInt32(ImageIds[i]));
                    Images.Add(image);
                }
            }
        }
        public string ImagesIdToCSV()
        {
            string str = "";
            foreach (Image i in Images)
            {
                str += i.Id + ",";
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }

        public string Print
        {
            get
            {
                return Name + ": " + Location.State + ", " + Location.City + ", " + AccommodationType + ", " + MaxGuestNumber + ", " + MinReservationDays;
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
