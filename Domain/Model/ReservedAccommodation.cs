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
using BookingApp.View.Guest.Pages;
using BookingApp.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BookingApp.Domain.Model
{
    public class ReservedAccommodation : ISerializable, INotifyPropertyChanged
    {
        private int id { get; set; }
        private int guestId { get; set; }
        private int guestNumber {  get; set; }
        private DateTime checkInDate { get; set; }
        private DateTime checkOutDate { get; set; }
        private Accommodation accommodation { get; set; }

        private List<Image> images { get; set; }
        private ObservableCollection<string> imagePaths { get; set; }
        private int currentImageIndex = 0;
        public string CurrentImagePath => ImagePaths?.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths?.Count ?? 0;
        public RelayCommand PreviousImageCommand => new RelayCommand(execute => PreviousImage(), canExecute => CanPreviousImage());
        public RelayCommand NextImageCommand => new RelayCommand(execute => NextImage(), canExecute => CanNextImage());
        public ReservedAccommodation() 
        {
            accommodation = new Accommodation();
            images = new List<Image>();
            imagePaths = new ObservableCollection<string>();
        }
        public ReservedAccommodation(int guestId, int accommodationId, DateTime checkInDate, DateTime checkOutDate)
        {
            this.guestId = guestId;
            this.checkInDate = checkInDate;
            this.checkOutDate = checkOutDate;
            accommodation = new Accommodation();
            accommodation = AccommodationService.GetInstance().GetById(accommodationId);
        }
        public ReservedAccommodation(ReservedAccommodation reservedAccommodation)
        {
            this.guestId = reservedAccommodation.guestId;
            this.checkInDate = reservedAccommodation.checkInDate;
            this.checkOutDate = reservedAccommodation.checkOutDate;
            accommodation = new Accommodation();
            accommodation = AccommodationService.GetInstance().GetById(reservedAccommodation.Accommodation.Id);
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
        public int GuestNumber
        {
            get
            {
                return guestNumber;
            }
            set
            {
                if (value != guestNumber)
                {
                    guestNumber = value;
                    OnPropertyChanged(nameof(guestNumber));
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
        public string ImagesIdToCSV()
        {
            string str = "";
            if (images.Count != 0)
            {
                foreach (Image i in Images)
                {
                    str += i.Id + ",";
                }
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                Accommodation.Id.ToString(),
                GuestId.ToString(),
                GuestNumber.ToString(),
                CheckInDate.ToString(),
                CheckOutDate.ToString(),
                ImagesIdToCSV()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            int accommodationId = Convert.ToInt32(values[1]);
            Accommodation = AccommodationService.GetInstance().GetById(accommodationId);
            GuestId = Convert.ToInt32(values[2]);
            GuestNumber = Convert.ToInt32(values[3]);
            CheckInDate = Convert.ToDateTime(values[4]);
            CheckOutDate = Convert.ToDateTime(values[5]);
            if (values[6].Length > 0)
            {
                string[] ImageIds = values[6].Split(',');
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
