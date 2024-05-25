using BookingApp.Repository;
using BookingApp.Serializer;
using BookingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum AccommodationType { Apartment, House, Hut }
namespace BookingApp.Domain.Model
{
    public class Accommodation : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        private int id { get; set; }
        private int ownerId { get; set; }
        private string name { get; set; }
        private Location? location { get; set; }
        private AccommodationType accommodationType { get; set; }
        private int maxGuestNumber { get; set; }
        private int minReservationDays { get; set; }
        private int cancelationDaysLimit { get; set; }
        private List<Image> images { get; set; }
        private string recommended { get; set; }

        private ObservableCollection<string> imagePaths { get; set; }
        private int currentImageIndex = 0;
        public string CurrentImagePath => ImagePaths?.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths?.Count ?? 0;
        public RelayCommand PreviousImageCommand => new RelayCommand(execute => PreviousImage(), canExecute => CanPreviousImage());
        public RelayCommand NextImageCommand => new RelayCommand(execute => NextImage(), canExecute => CanNextImage());
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public string Error => null;
        private const string SRB = "sr-RS";
        private Regex TextRegex = new Regex("^[A-Za-zČĆŠĐŽčćšđž ]+$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        return "*";

                    Match match = TextRegex.Match(Name);
                    if (!match.Success)
                        if (App.currentLanguage() == SRB)
                            return "Polje moze da sadrzi samo slova";
                        else
                            return "The field can only contain letters";
                }
                return null;
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
                    UpdateImagePaths();
                    OnPropertyChanged(nameof(images));
                }
            }
        }
        public ObservableCollection<string> ImagePaths
        {
            get
            {
                return imagePaths;
            }
            set
            {
                if (value != imagePaths)
                {
                    imagePaths = value;
                    OnPropertyChanged(nameof(imagePaths));
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
            ImagePaths = new ObservableCollection<string>();
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
                    ImagePaths.Add(image.Path);
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
        public string PrintAccommodation
        {
            get
            {
                return Name + ": " + Location.State + " - " + Location.City;
            }
            set
            {
                if (value != PrintAccommodation)
                {
                    PrintAccommodation = value;
                    OnPropertyChanged("PrintAccommodation");
                }
            }
        }
        private void UpdateImagePaths()
        {
            if (Images != null)
            {
                ImagePaths?.Clear();
                ImagePaths = new ObservableCollection<string>(Images.Select(image => image.Path));
            }
            else
            {
                ImagePaths = null;
            }
        }
        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set
            {
                if (value >= 0 && value < ImagePaths.Count)
                {
                    currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
        }
        public void NextImage()
        {
            if (CurrentImageIndex < TotalImages - 1)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanNextImage()
        {
            if (CurrentImageIndex == TotalImages - 1 || TotalImages == 0)
                return false;
            return true;
        }
        public void PreviousImage()
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanPreviousImage()
        {
            if (CurrentImageIndex == 0 || TotalImages == 0)
                return false;
            return true;
        }
    }
}
