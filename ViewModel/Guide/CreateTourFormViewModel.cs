using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;
using BookingApp.View.Guide.Pages;
using Image = BookingApp.Domain.Model.Image;
using BookingApp.Services;
using BookingApp.Domain.IRepositories;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;
using VirtualKeyboard.Wpf;
using Notification.Wpf;

namespace BookingApp.ViewModel.Guide
{
    public class CreateTourFormViewModel : INotifyPropertyChanged
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        private ObservableCollection<string> _relativeImagePaths = new ObservableCollection<string>();
        public ObservableCollection<string> RelativeImagePaths
        {
            get { return _relativeImagePaths; }
            set
            {
                _relativeImagePaths = value;
                OnPropertyChanged(nameof(RelativeImagePaths));
                // Update current image when the list changes
                if (_relativeImagePaths.Any())
                {
                    CurrentImage = _relativeImagePaths.First();
                }
                else
                {
                    CurrentImage = null;
                }
            }
        }
        private ObservableCollection<string> _ImagePaths = new ObservableCollection<string>();
        public ObservableCollection<string> ImagePaths
        {
            get { return _ImagePaths; }
            set
            {
                _ImagePaths = value;
                OnPropertyChanged(nameof(ImagePaths));
                // Update current image when the list changes
                if (_ImagePaths.Any())
                {
                    CurrentImage = _ImagePaths.First();
                }
                else
                {
                    CurrentImage = null;
                }
            }
        }
        private string _selectedKeyPoint;
        public string SelectedKeyPoint
        {
            get { return _selectedKeyPoint; }
            set
            {
                if (_selectedKeyPoint != value)
                {
                    _selectedKeyPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedState;
        public string SelectedState
        {
            get { return _selectedState; }
            set
            {
                if (_selectedState != value)
                {
                    _selectedState = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _hasEnoughKeypoints = false;
        public bool HasEnoughKeypoints
        {
            get { return _hasEnoughKeypoints; }
            set
            {
                _hasEnoughKeypoints = value;
                OnPropertyChanged();
            }
        }
        private bool _stateBoxIsEnabled = true;
        public bool StateBoxIsEnabled
        {
            get { return _stateBoxIsEnabled; }
            set
            {
                _stateBoxIsEnabled = value;
                OnPropertyChanged(nameof(StateBoxIsEnabled));
            }
        }
        private bool _cityBoxIsEnabled = false;
        public bool CityBoxIsEnabled
        {
            get { return _cityBoxIsEnabled; }
            set
            {
                _cityBoxIsEnabled = value;
                OnPropertyChanged(nameof(CityBoxIsEnabled));
            }
        }
        private bool _langaugeBoxIsEnabled = true;
        public bool LanguageBoxIsEnabled
        {
            get { return _langaugeBoxIsEnabled; }
            set
            {
                _langaugeBoxIsEnabled = value;
                OnPropertyChanged(nameof(LanguageBoxIsEnabled));
            }
        }

        private List<Image> images = new List<Image>();

        private List<TourSchedule> schedules = new List<TourSchedule>();
        public ObservableCollection<DateTime> Dates { get; set; } = new ObservableCollection<DateTime>();
        public List<KeyPoint> keyPoints = new List<KeyPoint>();
        public List<Location> Locations = new List<Location>();
        public ObservableCollection<string> KeyPointStrings { get; set; } = new ObservableCollection<string>();
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        public RelayCommand BtnSelectFile_Click => new RelayCommand(execute => BtnSelectFiles_ClickExecute());
        public RelayCommand ClickAddDate => new RelayCommand(execute => ClickAddDateExecute(), canExecute => ClickAddDateCanExecute());
        public RelayCommand ClidKAddKeyPoint => new RelayCommand(execute => ClickAddKeyPointExecute(),canExecute => ClickAddKeyPointCanExecute());
        public RelayCommand ClicKRemoveKeyPoint => new RelayCommand(execute => ClickRemoveKeyPointExecute(),canExecute => ClickRemoveKeyPointCanExecute());
        public RelayCommand ClickCancelButton => new RelayCommand(execute => ClickCancelButtonExecute());
        public RelayCommand ClickCreateButton => new RelayCommand(execute => ClickCreateButtonExecute(), canExecute => ClickCreateCanExecute());
        public RelayCommand ClickDeleteDate => new RelayCommand(execute => ClickDeleteDateExecute(), canExecute => ClickDeleteDateCanExecute());
        public RelayCommand LeftArrowCommand => new RelayCommand(execute => LeftArrowExecute(), canExecute => LeftArrowCanExecute());
        public RelayCommand RightArrowCommand => new RelayCommand(execute => RightArrowExecute(), canExecute => RightArrowCanExecute());
        public RelayCommand RemoveCommand => new RelayCommand(execute => RemoveExecute(), canExecute => RemoveCanExecute());
        public RelayCommand SetLocationCommand => new RelayCommand(execute => SetLocationCommandExecute());
        public RelayCommand SetLanguageCommand => new RelayCommand(execute => SetLanguageCommandExecute());

        private void SetLanguageCommandExecute()
        {
            Language = TourSuggestionService.GetInstance().GetMostRequestedLanguage();
            List<string> states = LocationService.GetInstance().GetAll().Select(t => t.State).Distinct().ToList();
            States.Clear();
            foreach(string state in states)
            {
                States.Add(state);
            }
            SelectedState = null;
            Cities.Clear();
            SelectedCity = null;
            StateBoxIsEnabled = true;
            CityBoxIsEnabled = false;
            LanguageBoxIsEnabled = false;
        }

        private void SetLocationCommandExecute()
        {
            int locationId = TourSuggestionService.GetInstance().GetMostRequestedLocation();
            Location location = LocationService.GetInstance().GetById(locationId);
            SelectedState = location.State;
            SelectedCity = location.City;
            Language = "";
            StateBoxIsEnabled = false;
            CityBoxIsEnabled = false;
            LanguageBoxIsEnabled = true;
        }

        private bool ClickDeleteDateCanExecute()
        {
            return Dates.Contains(SelectedDate);
        }

        private void ClickDeleteDateExecute()
        {
            Dates.Remove(SelectedDate);
        }


        private bool ClickCreateCanExecute()
        {
            if (KeyPointStrings.Count() < 2 || Dates.Count() < 1 || RelativeImagePaths.Count() < 1 ||
                Name.Equals("")  || Duration<1 || MaxTourists<1 || Description.Equals("") ||
                SelectedCity==null) 
            {
                return false; 
            }
            return true;
        }
        public List<int> HoursList { get; set; }
        public List<int> MinutesList { get; set; }
        public List<string> AmPm { get; set; }

        public ObservableCollection<string> States { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        private int _hours = 1;
        public int SelectedHour
        {
            get => _hours;
            set
            {
                if (value != _hours)
                {
                    _hours = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _minutes;
        public int SelectedMinute
        {
            get => _minutes;
            set
            {
                if (value != _minutes)
                {
                    _minutes = value;
                    OnPropertyChanged();
                }
            }
        }
        private List<string> toBeDeleted = new List<string>();
        private string _name= "";
        public new string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _duration=1;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    bool isInteger = int.TryParse(value.ToString(), out _);
                    IsDurationInteger = isInteger;
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _maxTourists=1;
        public int MaxTourists
        {
            get => _maxTourists;
            set
            {
                if (value != _maxTourists)
                {
                    bool isInteger = int.TryParse(value.ToString(), out _);
                    IsTouristsInteger = isInteger;
                    _maxTourists = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isTouristsInteger;
        public bool IsTouristsInteger
        {
            get => _isTouristsInteger;
            set
            {
                if (value != _isTouristsInteger)
                {
                    _isTouristsInteger = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isDurationInteger;
        public bool IsDurationInteger
        {
            get => _isDurationInteger;
            set
            {
                if (value != _isDurationInteger)
                {
                    _isDurationInteger = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _currentKeyPoint;
        public string CurrentKeyPoint
        {
            get => _currentKeyPoint;
            set
            {
                if (value != _currentKeyPoint)
                {
                    _currentKeyPoint = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _state;
        public string State
        {
            get => _state;
            set
            {
                if (value != _state)
                {
                    _state = value;
                    OnPropertyChanged();
                }
            }
        }
        public static bool IsCreated=false;
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public new string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _ampm = "AM";
        public new string SelectedAmPm
        {
            get => _ampm;
            set
            {
                if (value != _ampm)
                {
                    _ampm = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _currentImage;
        public string CurrentImage
        {
            get => _currentImage;
            set
            {
                if (value != _currentImage)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }
        private int currentImageIndex;
        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set
            {
                if (value >= 0 && value < RelativeImagePaths.Count)
                {
                    currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
        }
        public string CurrentImagePath;
        public int TotalImages => RelativeImagePaths.Count;

        private string _description="";

        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private CreateTourForm createTourForm;
        public Action OnClickedGoBack { get; set; }
        public CreateTourFormViewModel() { }
        public void CheckConversions(object o,TextChangedEventArgs e)
        {
        }
        public CreateTourFormViewModel(CreateTourForm createTourForm, User user)
        {
            this.createTourForm=createTourForm;
            CurrentImage = "../../../Resources/Images/No-Image-Placeholder.png";
            createTourForm.StateBoxEventHandler += StateBox_SelectionChanged;
            User = user;
            images.Clear();
            HoursList = Enumerable.Range(1, 12).ToList();
            MinutesList = new List<int>() { 0, 15, 30, 45 };
            AmPm = new List<string>() { "AM", "PM" };
            Locations = LocationService.GetInstance().GetAll();
            States = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            createTourForm.datePicker.DisplayDateStart = DateTime.Now;
            User = user;
            foreach (Location location in Locations)
            {
                if (!States.Contains(location.State))
                {
                    States.Add(location.State);
                }
            }
        }
        public User User { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void BtnSelectFiles_ClickExecute()
        {
            RelativeImagePaths.Clear();
            ImagePaths.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                // Get path to .exe file
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                // Position to the right folder
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\Tour";

                // Make sure that the target folder exists
                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }
                foreach (string filePath in dlg.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    images.Clear();
                    fileName = SaveImageFile(filePath, destFilePath, fileName);
                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Tour/", fileName);
                    RelativeImagePaths.Add(relativePath);
                    ImagePaths.Add(destFilePath);
                    CurrentImage = ImagePaths[0];
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }
        public void SaveImageIntoCSV(ObservableCollection<string> relativeImagePaths)
        {
            foreach (string filePath in relativeImagePaths)
            {
                Image image = new Image(0, filePath);
                image = ImageService.GetInstance().Add(image);
                images.Add(image);
            }
        }
        public string SaveImageFile(string filePath, string destFilePath, string fileName)
        {
            if (!File.Exists(destFilePath))
            {
                File.Copy(filePath, destFilePath, false);
                return fileName;
            }
            else
            {
                string[] fileNameParts = fileName.Split(".");
                while (File.Exists(destFilePath))
                {
                    string[] name = destFilePath.Split(".");
                    name[0] = name[0] + "A";
                    destFilePath = name[0] + "." + name[1];
                    fileNameParts[0] = fileNameParts[0] + "A";
                }
                File.Copy(filePath, destFilePath, false);
                return fileNameParts[0] + "." + fileNameParts[1];
            }
        }
        public string GetBaseFolder(string path)
        {
            for (int i = 0; i < 4; i++)
            {
                DirectoryInfo? parentDirectory = Directory.GetParent(path);
                if (parentDirectory != null)
                {
                    path = parentDirectory.FullName;
                }
                else
                {
                    throw new InvalidOperationException("Cannot navigate up three directories from base.");
                }
            }
            return path;
        }

        public void ClickCreateButtonExecute()
        {

            // Create a new Tour object
            State = this.SelectedState;
            City = this.SelectedCity;
            Location location = new Location(SelectedCity, SelectedState);
            location.Id = LocationService.GetInstance().GetIdByStateCity(SelectedState, SelectedCity);
            Tour newTour = new Tour
            {
                Name = this.Name,
                Description = this.Description,
                Language = Language,
                Location = location,
                MaxTourists = this.MaxTourists,
                Duration = this.Duration,
                DateTime = DateTime.Now,
                Images = images,
                KeyPoints = this.keyPoints,
                OwnerId = User.Id
            };
            newTour.LocationId = location.Id;
            try
            {
                if (RelativeImagePaths.Count > 0)
                {
                    SaveImageIntoCSV(RelativeImagePaths);
                }
                newTour = TourService.GetInstance().Add(newTour);
                int tourId = newTour.Id;
                TourNotificationService.GetInstance().SendNotifications(newTour);
                foreach (string keyPointString in KeyPointStrings)
                {
                    KeyPoint keyPoint = new KeyPoint(tourId, keyPointString);
                    KeyPointService.GetInstance().Add(keyPoint);
                }
                foreach (Image image in images)
                {
                    TourImage tourImage = new TourImage(tourId, image.Id);
                    TourImageService.GetInstance().Add(tourImage);
                }
                foreach (DateTime date in Dates)
                {
                    TourSchedule schedule = new TourSchedule();
                    schedule.TourId = tourId;
                    schedule.Date = date;
                    TourScheduleService.GetInstance().Add(schedule);
                }
                IsCreated = true;
                notificationManager.Show("Success", "You have successfully created a tour!", NotificationType.Success);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create the tour. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnClickedGoBack?.Invoke();
            createTourForm.NavigationService.GoBack();
        }

        public void ClickCancelButtonExecute()
        {
            Dates.Clear();
            keyPoints.Clear();
            schedules.Clear();
            foreach(string path in RelativeImagePaths)
            {
                toBeDeleted.Add(path);
            }
            RelativeImagePaths.Clear();
            images.Clear();
            KeyPointStrings.Clear();
            Locations.Clear();
            OnClickedGoBack?.Invoke();
            this.createTourForm.NavigationService.GoBack();
        }
        public void ClickAddKeyPointExecute()
        {
            KeyPointStrings.Add(this.CurrentKeyPoint);
            this.CurrentKeyPoint = "";
            if (KeyPointStrings.Count > 1)
            {
                HasEnoughKeypoints=true;
            }
        }
        private bool ClickAddKeyPointCanExecute()
        {
            if(CurrentKeyPoint != null) 
            {
                if (!CurrentKeyPoint.Equals(""))
                {
                    return true;
                }
            }
            return false;
        }
        private void ClickRemoveKeyPointExecute()
        {
            KeyPointStrings.Remove(this.SelectedKeyPoint);
            if (KeyPointStrings.Count < 2)
            {
                HasEnoughKeypoints = false;
            }
        }
        private bool ClickRemoveKeyPointCanExecute()
        {
            if(SelectedKeyPoint != null)
            {
                return true;
            }
            return false;
        }
        public bool ClickAddDateCanExecute()
        {
            try
            {
                if(this.SelectedHour == null || this.SelectedAmPm == null || this.SelectedMinute == null ||this.SelectedDate == null)
                {
                    return false;
                }
                int selectedHour = SelectedHour;
                int selectedMinute = SelectedMinute;
                string selectedAmPm = SelectedAmPm;
                DateTime date = SelectedDate;
                if (selectedAmPm == "PM" && selectedHour != 12)
                {
                    selectedHour += 12;
                }
                else if (selectedAmPm == "AM" && selectedHour == 12)
                {
                    selectedHour = 0;
                }
                DateTime selectedDate = new DateTime(date.Year, date.Month, date.Day, selectedHour, selectedMinute, 0);
                if (Dates.Where(t => t.Date == selectedDate.Date && t.Minute == selectedDate.Minute && t.Hour == selectedDate.Hour).Count() == 0)
                {
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        public void ClickAddDateExecute()
        {
            int selectedHour = SelectedHour;
            int selectedMinute = SelectedMinute;
            string selectedAmPm = SelectedAmPm;
            if (selectedAmPm == "PM" && selectedHour != 12)
            {
                selectedHour += 12;
            }
            else if (selectedAmPm == "AM" && selectedHour == 12)
            {
                selectedHour = 0;
            }

            DateTime date = SelectedDate;
            DateTime selectedDate = new DateTime(date.Year, date.Month, date.Day, selectedHour, selectedMinute, 0);
            if(Dates.Where(t=>t.Date==selectedDate.Date && t.Minute==selectedDate.Minute && t.Hour == selectedDate.Hour).Count() == 0)
            {
                Dates.Add(selectedDate);
            }
        }
        public bool DateExists(DateTime date)
        {
            foreach (DateTime listDate in Dates)
            {
                if (date.Date == listDate.Date && date.Hour.ToString().Equals(listDate.Hour.ToString()) && date.Minute.ToString().Equals(listDate.Minute.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        private int _currentIndex = 0;
        private void LeftArrowExecute()
        {
            if (ImagePaths.Count > 0)
            {
                _currentIndex--;
                CurrentImage = ImagePaths[_currentIndex];
                OnPropertyChanged(nameof(CurrentImage));
            }
        }
        private bool LeftArrowCanExecute()
        {
            if (_currentIndex > 0)
            {
                return true;
            }
            return false;
        }
        private void RightArrowExecute()
        {
            _currentIndex++;
            CurrentImage = ImagePaths[_currentIndex];
            OnPropertyChanged(nameof(CurrentImage));
        }
        private bool RightArrowCanExecute()
        {
            if (_currentIndex < ImagePaths.Count - 1)
            {
                return true;
            }
            return false;
        }
        private void RemoveExecute()
        {
            toBeDeleted.Add(ImagePaths[_currentIndex]);
            RelativeImagePaths.RemoveAt(_currentIndex);
            ImagePaths.RemoveAt(_currentIndex);
            if(ImagePaths.Count == 0)
            {
                CurrentImage = "../../../Resources/Images/No-Image-Placeholder.png";
                return;
            }
            else if(_currentIndex > ImagePaths.Count - 1)
            {
                CurrentImage= ImagePaths[ImagePaths.Count - 1];
                _currentIndex=ImagePaths.Count - 1;
            }
            else
            {
                CurrentImage = ImagePaths[_currentIndex];
            }
        }
        private bool RemoveCanExecute() 
        {
            if (ImagePaths.Count() > 0 && _currentIndex < ImagePaths.Count())
            {
                return true;
            }
            return false;
        }

        public void StateBox_SelectionChanged()
        {
            Cities.Clear();
            if(this.SelectedState != null)
            {
            foreach (Location location in Locations)
            {
                if (this.SelectedState.Equals(location.State) && !Cities.Contains(location.City))
                {
                    Cities.Add(location.City);
                }
            }
                SelectedCity = Cities.First();
                CityBoxIsEnabled= true;
            }
            else
            {
                CityBoxIsEnabled= false;
                StateBoxIsEnabled= true;
            }
        }
    }
}
