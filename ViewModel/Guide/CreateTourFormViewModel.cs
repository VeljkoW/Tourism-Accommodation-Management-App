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

namespace BookingApp.ViewModel.Guide
{
    public class CreateTourFormViewModel
    {
        public List<string> relativeImagePaths = new List<string>();
        private List<Image> images = new List<Image>();
        private List<TourSchedule> schedules = new List<TourSchedule>();
        private List<DateTime> dates = new List<DateTime>();
        public List<KeyPoint> keyPoints = new List<KeyPoint>();
        public List<Location> Locations = new List<Location>();
        public List<string> keyPointStrings = new List<string>();

        public RelayCommand BtnSelectFile_Click => new RelayCommand(execute => BtnSelectFiles_ClickExecute());
        public RelayCommand ClickAddDate => new RelayCommand(execute => ClickAddDateExecute());
        public RelayCommand ClidKAddKeyPoint => new RelayCommand(execute => ClickAddKeyPointExecute());
        public RelayCommand ClickCancelButton => new RelayCommand(execute => ClickCancelButtonExecute());
        public RelayCommand ClickCreateButton => new RelayCommand(execute => ClickCreateButtonExecute(), canExecute => ClickCreateCanExecute());


        private bool ClickCreateCanExecute()
        {
            if (keyPointStrings.Count() < 2 || dates.Count() < 1 || relativeImagePaths.Count() < 1) {  return false; }
            return true;
        }

        //new RelayCommand(execute => AcceptExecute(), canExecute => AcceptCanExecute());
        public List<int> HoursList { get; set; }
        public List<int> MinutesList { get; set; }
        public List<string> AmPm { get; set; }

        public List<string> States { get; set; }
        public List<string> Cities { get; set; }
        private int _hours;
        public int Hours
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
        public int Minutes
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
        private string _name;
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
        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _maxTourists;
        public int MaxTourists
        {
            get => _maxTourists;
            set
            {
                if (value != _maxTourists)
                {
                    _maxTourists = value;
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
        private string _description;
        private CreateTourForm createTourForm;
        public CreateTourFormViewModel() { }
        public CreateTourFormViewModel(CreateTourForm createTourForm, User user)
        {
            this.createTourForm = createTourForm;
            createTourForm.StateBoxEventHandler += StateBox_SelectionChanged;
            User = user;
            images.Clear();
            HoursList = Enumerable.Range(1, 12).ToList();
            MinutesList = new List<int>() { 0, 15, 30, 45 };
            AmPm = new List<string>() { "AM", "PM" };
            Locations = LocationService.GetInstance().GetAll();
            States = new List<string>();
            Cities = new List<string>();
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
        public User User { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BtnSelectFiles_ClickExecute()
        {
            relativeImagePaths.Clear();
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
                    relativeImagePaths.Add(relativePath);
                }
            }
        }
        public void SaveImageIntoCSV(List<string> relativeImagePaths)
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
            State = createTourForm.StateBox.Text.Trim();
            City = createTourForm.CityBox.Text.Trim();
            Location location = new Location(State, City);
            location.Id = LocationService.GetInstance().GetIdByStateCity(State, City);
            if (ValidateTextboxes() == false)
            {
                return;
            }
            Tour newTour = new Tour
            {
                Name = createTourForm.TourNameTextbox.Text.Trim(),
                Description = createTourForm.DescriptionTextbox.Text.Trim(),
                Language = createTourForm.LanguageTextbox.Text.Trim(),
                Location = location,
                MaxTourists = Convert.ToInt32(createTourForm.MaxTouristTextbox.Text.Trim()),
                Duration = Convert.ToInt32(createTourForm.DurationTextbox.Text.Trim()),
                DateTime = DateTime.Now,
                Images = images,
                KeyPoints = this.keyPoints,
                OwnerId = User.Id
            };
            newTour.LocationId = location.Id;
            try
            {
                if (relativeImagePaths.Count > 0)
                {
                    SaveImageIntoCSV(relativeImagePaths);
                }
                newTour = TourService.GetInstance().Add(newTour);
                int tourId = newTour.Id;
                foreach (string keyPointString in keyPointStrings)
                {
                    KeyPoint keyPoint = new KeyPoint(tourId, keyPointString);
                    KeyPointService.GetInstance().Add(keyPoint);
                }
                foreach (Image image in images)
                {
                    TourImage tourImage = new TourImage(tourId, image.Id);
                    TourImageService.GetInstance().Add(tourImage);
                }
                foreach (DateTime date in dates)
                {
                    TourSchedule schedule = new TourSchedule();
                    schedule.TourId = tourId;
                    schedule.Date = date;
                    TourScheduleService.GetInstance().Add(schedule);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create the tour. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            createTourForm.NavigationService.GoBack();
        }

        public bool ValidateTextboxes()
        {
            if (relativeImagePaths.Count == 0 || string.IsNullOrEmpty(createTourForm.TourNameTextbox.Text) || keyPointStrings.Count < 2)
            {
                if (images.Count == 0)
                {
                    createTourForm.ErrorCode.Text = "No Image selected";
                }
                if (string.IsNullOrEmpty(createTourForm.TourNameTextbox.Text))
                {
                    if (string.IsNullOrEmpty(createTourForm.ErrorCode.Text))
                    {
                        createTourForm.ErrorCode.Text = "Missing name";
                    }
                    else
                    {
                        createTourForm.ErrorCode.Text += ", missing name";
                    }
                }
                if (keyPointStrings.Count < 2)
                {
                    if (string.IsNullOrEmpty(createTourForm.ErrorCode.Text))
                    {
                        createTourForm.ErrorCode.Text = "Missing keypoints";
                    }
                    else
                    {
                        createTourForm.ErrorCode.Text += ", missing keypoints";
                    }
                }
                return false;
            }
            return true;
        }

        public void ClickCancelButtonExecute()
        {
            dates.Clear();
            keyPoints.Clear();
            schedules.Clear();
            relativeImagePaths.Clear();
            images.Clear();
            keyPointStrings.Clear();
            Locations.Clear();
            createTourForm.NavigationService.GoBack();
        }
        public void ClickAddKeyPointExecute()
        {
            keyPointStrings.Add(createTourForm.KeyPointTextbox.Text);
            createTourForm.KeyPointTextbox.Clear();
        }

        public void ClickAddDateExecute()
        {
            int selectedHour = (int)createTourForm.ChosenHours.SelectedValue;
            int selectedMinute = (int)createTourForm.ChosenMinutes.SelectedValue;
            string selectedAmPm = (string)createTourForm.ChosenAmPm.SelectedValue;
            if (selectedAmPm == "PM" && selectedHour != 12)
            {
                selectedHour += 12;
            }
            else if (selectedAmPm == "AM" && selectedHour == 12)
            {
                selectedHour = 0;
            }

            DateTime date = createTourForm.datePicker.SelectedDate ?? DateTime.Now.Date;
            DateTime selectedDate = new DateTime(date.Year, date.Month, date.Day, selectedHour, selectedMinute, 0);
            if (!DateExists(selectedDate))
            {
                dates.Add(selectedDate);
            }
        }
        public bool DateExists(DateTime date)
        {
            foreach (DateTime listDate in dates)
            {
                if (date.Date == listDate.Date && date.Hour.ToString().Equals(listDate.Hour.ToString()) && date.Minute.ToString().Equals(listDate.Minute.ToString()))
                {
                    return true;
                }
            }
            return false;
        }


        public void StateBox_SelectionChanged()
        {
            Cities.Clear();
            string selectedState = createTourForm.StateBox.SelectedItem as string;
            foreach (Location location in Locations)
            {
                if (selectedState.Equals(location.State) && !Cities.Contains(location.City))
                {
                    Cities.Add(location.City);
                }
            }
            createTourForm.CityBox.ItemsSource = null;
            createTourForm.CityBox.ItemsSource = Cities;
            createTourForm.CityBox.IsEnabled = createTourForm.StateBox.SelectedItem != null;
        }
    }
}
